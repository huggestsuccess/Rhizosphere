namespace Rhizosphere.Core;

public class RhizosphereHandler : BackgroundService
{
    private readonly ILogger<RhizosphereHandler> _log;
    private readonly RhizosphereState _state;
    private readonly ClimateService _cs;
    private readonly Fan _fan;
    private readonly FogMachine _fm;

    public RhizosphereHandler
        (
            ILogger<RhizosphereHandler> logger,
            RhizosphereState state,
            ClimateService cs,
            Fan fan,
            FogMachine fogMachine
        )
    {
        _log = logger;
        _state = state;
        _cs = cs;
        _fan = fan;
        _fm = fogMachine;
    }

    protected override async Task ExecuteAsync(CancellationToken token = default)
    {
        while (!token.IsCancellationRequested)
        {
            try
            {
                await Task.Delay(500, token);

                if(_state.ManualMode)
                    continue;

                HandleRhizosphere();
            }
            catch (TaskCanceledException)
            {

            }
            catch (Exception ex)
            {
                _log.LogError("Severe error: {ex}", ex);
            }
        }
    }

    private void HandleRhizosphere()
    {
        HandleFan(_state.ActivePhase);
        HandleFogMachine(_state.ActivePhase);
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
}