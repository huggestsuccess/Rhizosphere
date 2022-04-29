namespace Rhizosphere.Core;
public class ClimateService : BackgroundService
{
    private readonly ILogger<ClimateService> _log;
    private readonly Dht22 _dht;

    public ClimateService(ILogger<ClimateService> logger)
    {
        _log = logger;
        _dht = new Dht22(14);
    }

    protected override async Task ExecuteAsync(CancellationToken token = default)
    {

        while (!token.IsCancellationRequested)
        {
            await Task.Delay(5000, token);

            if (_dht.TryReadHumidity(out var relativeHumidity))
                _log.LogInformation("Read {r} {p}", relativeHumidity, relativeHumidity.Percent);
            else
                _log.LogWarning("Unable to read humidity");

            if (_dht.TryReadTemperature(out var temperature))
                _log.LogInformation("Read {r} {c}", temperature, temperature.DegreesCelsius);
            else
                _log.LogWarning("Unable to read temperature");
        }
    }
}