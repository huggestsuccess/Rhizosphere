namespace Rhizosphere.Core;


public class UptimeLimit
{
    public TimeSpan? Daily {get; set;} 
    public TimeSpan? Hourly {get; set;} 
    public TimeSpan? Minutely {get; set;} 
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

        while (!token.IsCancellationRequested)
        {
            await Task.Delay(5000, token);


            bool shouldFanRun = GetWetherFanShouldRun(_cs);
            
            if(shouldFanRun == _fan.IsRunning)
                continue;

            
            if(shouldFanRun)
            {
                _log.LogWarning("Its getting too hot, turning on fan at {t}", _cs.LatestTemperature?.Value);

                _fan.Run();
            }
            else
            {
                _log.LogWarning("Fan has run long enugh,turning it off at {t}", _cs.LatestTemperature?.Value);

                _fan.Stop();
            }
        }
    }

    private bool GetWetherFanShouldRun(ClimateService cs)
        => cs.LatestRelativeHumidity is not null
        && cs.LatestTemperature is not null
        && cs.LatestTemperature.Value.Value > 23;

    pivate bool IsWithinUptimeLimit<T>(Device<T> device, )
}