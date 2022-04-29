namespace Rhizosphere.Core;

public record Read<T>(T Value, DateTime Timestamp);


public class ClimateService : BackgroundService
{
    private readonly ILogger<ClimateService> _log;
    private readonly Dht22 _dht;

    public ClimateService(ILogger<ClimateService> logger)
    {
        _log = logger;
        _dht = new Dht22(14);
    }

    public Read<RelativeHumidity>? LatestRelativeHumidity { get; private set; }

    public Read<Temperature>? LatestTemperature { get; private set; }

    protected override async Task ExecuteAsync(CancellationToken token = default)
    {

        while (!token.IsCancellationRequested)
        {
            await Task.Delay(5000, token);

            if (!_dht.TryReadHumidity(out var relativeHumidity))
                _log.LogWarning("Unable to read humidity");
            else
            {
                _log.LogInformation("Read {r} {p}", relativeHumidity, relativeHumidity.Percent);

                LatestRelativeHumidity = new Read<RelativeHumidity>(relativeHumidity, DateTime.Now);
            }

            if (!_dht.TryReadTemperature(out var temperature))
                _log.LogWarning("Unable to read temperature");
            else
            {
                _log.LogInformation("Read {r} {c}", temperature, temperature.DegreesCelsius);

                LatestTemperature = new Read<Temperature>(temperature, DateTime.Now);
            }
        }
    }
}