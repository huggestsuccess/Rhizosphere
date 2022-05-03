namespace Rhizosphere.Core;



public class RhizosphereHandler : BackgroundService
{
    private readonly ILogger<RhizosphereHandler> _log;
    private readonly ClimateService _cs;
    private readonly Fan _fan;

    public RhizosphereHandler(ILogger<RhizosphereHandler> logger, ClimateService cs, Fan fan)
    {
        _log = logger;
        _cs = cs;
        _fan = fan;
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
}