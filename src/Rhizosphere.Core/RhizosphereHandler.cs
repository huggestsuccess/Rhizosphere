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
    public TimeSpan Daily {get; set;}
    public TimeSpan Hourly {get; set;}
    public TimeSpan Minutely {get; set;}

    public Uptime()
    {

    }

    public Uptime(Uptime previous, DateTime from, DateTime to)
    {   
        var dt = from - to;

        if(from.Minute == to.Minute)
            Minutely+= dt;
        else    
            Minutely = TimeSpan.FromSeconds(dt.Seconds);

        if(from.Hour == to.Hour)
            Hourly += dt;
        else    
            Hourly = TimeSpan.FromSeconds(dt.Minutes);

        if(from.Day == to.Day)
            Daily+= dt;
        else    
            Daily = TimeSpan.FromSeconds(dt.Seconds);
    }


    public override string? ToString()
        => $"m{Minutely}h{Hourly}d{Daily}";
}

public class DeviceOptions
{
    public int PinNumber { get; set; }  

    public bool NormallyOpen { get; set; }  = false;

    public bool Enabled { get; set; }  = true;
    
    public bool ActiveOverride { get; set; }  = false;

    public UptimeLimit? UptimeLimit {get; set;}
}

public class RhizosphereOptions
{
    public string ActiveRecipeName {get; set;} = string.Empty;
    public FanOptions FanOptions {get; set;} = new();
    public FogMachineOptions FogMachineOptions {get; set;} = new();
}

public class RhizosphereHandler : BackgroundService
{
    private readonly ILogger<RhizosphereHandler> _log;
    private readonly ClimateService _cs;
    private readonly Fan _fan;

    private readonly FogMachine _fm;

    public RhizosphereHandler(ILogger<RhizosphereHandler> logger, ClimateService cs, Fan fan, FogMachine fogMachine)
    {
        _log = logger;
        _cs = cs;
        _fan = fan;
        _fm = fogMachine;
    }

    protected override async Task ExecuteAsync(CancellationToken token = default)
    {
        _log.LogInformation($"Fan uptime limit: {_fan.DeviceOptions.UptimeLimit}");
        _log.LogInformation($"FogMachine uptime limit: {_fm.DeviceOptions.UptimeLimit}");


        while (!token.IsCancellationRequested)
        {
            await Task.Delay(500, token);

            bool shouldFanRun = GetWetherFanShouldRun(_cs);

            var fanUptimeGood = IsWithinUptimeLimit(_fan);

            if(shouldFanRun == _fan.IsRunning && fanUptimeGood) 
                continue;
                
            if(fanUptimeGood)
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
    }

    private bool GetWetherFanShouldRun(ClimateService cs)
        => cs.LatestRelativeHumidity is not null
        && cs.LatestTemperature is not null
        && cs.LatestTemperature.Value.Value > 23;

    private bool IsWithinUptimeLimit(Device device)
    {
        var limit = device.DeviceOptions.UptimeLimit;

        if(limit is null)
            return true;

        var uptime = device.Uptime;

        if(uptime.Minutely > limit.Minutely)
            return false;

        if(uptime.Hourly > limit.Hourly)
            return false;

        if(uptime.Daily > limit.Daily)
            return false;
        
        return true;
    }
}