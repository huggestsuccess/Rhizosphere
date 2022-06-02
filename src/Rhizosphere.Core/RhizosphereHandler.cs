using Microsoft.Extensions.Options;

namespace Rhizosphere.Core;


public class UptimeLimit : Uptime
{
    public UptimeLimit() //better defaults
    {
        Daily = TimeSpan.FromDays(1);
        Hourly = TimeSpan.FromHours(1);
        Minutely = TimeSpan.FromMinutes(1);
    }
}

public class Uptime
{
    public TimeSpan Daily { get; set; }
    public TimeSpan Hourly { get; set; }
    public TimeSpan Minutely { get; set; }

    public Uptime()
    {

    }

    public Uptime(Uptime previous, DateTime from, DateTime to, bool isRunning)
    {
        if (isRunning)
        {
            var dt = to - from;

            if (from.Minute == to.Minute)
                Minutely += dt;
            else
                Minutely = TimeSpan.FromSeconds(dt.Seconds);

            if (from.Hour == to.Hour)
                Hourly += dt;
            else
                Hourly = TimeSpan.FromSeconds(dt.Minutes);

            if (from.Day == to.Day)
                Daily += dt;
            else
                Daily = TimeSpan.FromSeconds(dt.Seconds);
        }
        else
        {
            if (from.Minute == to.Minute)
                Minutely = previous.Minutely;
            else
                Minutely = default;

            if (from.Hour == to.Hour)
                Hourly = previous.Minutely;
            else
                Hourly = default;

            if (from.Day == to.Day)
                Daily = previous.Daily;
            else
                Daily = default;
        }
    }


    public override string? ToString()
        => $"D{Daily}|h{Hourly}|m{Minutely}";
}

public class DeviceOptions
{
    public int PinNumber { get; set; }

    public bool NormallyOpen { get; set; } = false;

    public bool Enabled { get; set; } = true;

    public bool ActiveOverride { get; set; } = false;

    public UptimeLimit? UptimeLimit { get; set; }
}

public class RhizosphereOptions
{
    public string ActiveRecipeName { get; set; } = string.Empty;
    public string ActivePhaseName { get; set; } = string.Empty;
    public FanOptions FanOptions { get; set; } = new();
    public FogMachineOptions FogMachineOptions { get; set; } = new();
}

public class RhizosphereHandler : BackgroundService
{
    private readonly ILogger<RhizosphereHandler> _log;
    private readonly IOptionsMonitor<RhizosphereOptions> _opt;
    private readonly RecipeRepository _rr;
    private readonly ClimateService _cs;
    private readonly Fan _fan;
    private readonly FogMachine _fm;

    public RhizosphereHandler
        (
            ILogger<RhizosphereHandler> logger,
            IOptionsMonitor<RhizosphereOptions> options,
            RecipeRepository recipeRepository,
            ClimateService cs,
            Fan fan,
            FogMachine fogMachine
        )
    {
        _log = logger;
        _opt = options;
        _rr = recipeRepository;
        _cs = cs;
        _fan = fan;
        _fm = fogMachine;
    }

    public Phase ActivePhase { get; set; } = new();

    protected override async Task ExecuteAsync(CancellationToken token = default)
    {
        while (!token.IsCancellationRequested)
        {
            await Task.Delay(500, token);

            var activePhase = GetActivePhase();

            if (activePhase is null)
                activePhase = new();

            if (ActivePhase.Name != activePhase.Name)
            {
                _log.LogWarning("Active Phase change! Active Phase name: {apn}", activePhase.Name);
                ActivePhase = activePhase;
            }

            HandleFan(activePhase);
            HandleFogMachine(activePhase);
        }
    }

    private void HandleFan(Phase activePhase)
    {
        bool shouldFanRun = GetWetherFanShouldRun(_fan, _cs, activePhase);

        if (shouldFanRun == _fan.IsRunning)
            return;

        if (shouldFanRun)
        {
            _log.LogWarning("Its getting too hot, turning on fan at {t}", _cs.LatestTemperature?.Value);

            _fan.Run();
        }
        else
        {
            _log.LogWarning("Fan has run long enugh,turning it off at {t}, {u}", _cs.LatestTemperature?.Value, _fan.Uptime);

            _fan.Stop();
        }
    }
    private void HandleFogMachine(Phase activePhase)
    {
        bool shouldFmRun = GetWetherFogMachineShouldRun(_fm, _cs, activePhase);


        if (shouldFmRun == _fm.IsRunning)
            return;

        if (shouldFmRun)
        {
            _log.LogWarning("Its getting too dry, turning on FogMachine at {h}", _cs.LatestRelativeHumidity?.Value);

            _fm.Run();
        }
        else
        {
            _log.LogWarning("FogMachine has run long enugh,turning it off at {h}, {u}", _cs.LatestRelativeHumidity?.Value, _fm.Uptime);

            _fm.Stop();
        }
    }

    private bool GetWetherFanShouldRun(Fan fan, ClimateService cs, Phase activePhase)
        => cs.LatestTemperature is not null
        && cs.LatestTemperature.Value.Value > activePhase.MinTemperature
        &&
        (
            cs.LatestTemperature.Value.Value > activePhase.MaxTemperature
                ||
            (
                cs.LatestRelativeHumidity is not null &&
                cs.LatestRelativeHumidity.Value.Value > activePhase.MinHumidity
            )
        )
        && fan.IsWithinUptimeLimit();

    private bool GetWetherFogMachineShouldRun(FogMachine fm, ClimateService cs, Phase activePhase)
        => cs.LatestRelativeHumidity is not null
        && cs.LatestRelativeHumidity.Value.Value < activePhase.MinHumidity
        && cs.LatestRelativeHumidity.Value.Value < activePhase.MaxHumidity
        && fm.IsWithinUptimeLimit();

    private Recipe? GetActiveRecipe()
        => _rr.GetRecipe(_opt.CurrentValue.ActiveRecipeName);

    private Phase? GetActivePhase()
    {
        var ar = GetActiveRecipe();

        if (ar is null)
            return null;

        return ar.Phases.FirstOrDefault(f => f.Name == _opt.CurrentValue.ActivePhaseName);
    }
}