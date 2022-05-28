namespace Rhizosphere.Controllers;

[ApiController]
[Route("[controller]")]
public class StatusController : ControllerBase
{
    private readonly ILogger<StatusController> _log;
    private readonly Fan _fan;
    private readonly FogMachine _fm;
    private readonly ClimateService _cs;

    public StatusController(ILogger<StatusController> logger, Fan fan, ClimateService climateService, FogMachine fogMachine)
    {
        _log = logger;
        _fan = fan;
        _cs = climateService;
        _fm = fogMachine;
    }

    [HttpGet]
    [Route("/Run")]
    public IActionResult Run()
        => Ok(_fan.Run());


    [HttpGet]
    [Route("/Stop")]
    public IActionResult Stop()
        => Ok(_fan.Stop());

    [HttpGet]
    public IActionResult Get()
    {
        var status = new RhizosphereStatus
        (
            _fan.IsRunning,
            _cs.LatestTemperature?.Value.DegreesCelsius,
            _cs.LatestTemperature?.Timestamp,

            _cs.LatestRelativeHumidity?.Value.Value,
            _cs.LatestRelativeHumidity?.Timestamp
        );
        return Ok(status);
    }
}

public record RhizosphereStatus
(
    bool FanRunning,
    double? TemperatureCelsius,
    DateTime? LatestTemperatureRead,
    double? HumidityPercentage,
    DateTime? LatestHumidityRead
);
