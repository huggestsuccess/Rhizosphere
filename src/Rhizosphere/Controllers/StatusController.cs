namespace Rhizosphere.Controllers;

[ApiController]
[Route("[controller]")]
public class StatusController : ControllerBase
{
    private readonly ILogger<StatusController> _log;
    private readonly IOptionsMonitor<RhizosphereOptions> _om;
    private readonly Fan _fan;
    private readonly FogMachine _fm;
    private readonly ClimateService _cs;
    

    public StatusController(ILogger<StatusController> logger, IOptionsMonitor<RhizosphereOptions> optionsMonitor,  Fan fan, ClimateService climateService, FogMachine fogMachine)
    {
        _log = logger;
        _fan = fan;
        _cs = climateService;
        _fm = fogMachine;
        _om = optionsMonitor;
    }
    [HttpGet]
    [Route("/Auto")]
    public IActionResult SetAutoMode()
        => Ok("TODO");

    [HttpGet]
    [Route("/Manual")]
    public IActionResult SetManualMode()
        => Ok("TODO");


    [HttpGet]
    [Route("/Fan/Run")]
    public IActionResult RunFan()
        => Ok(_fan.Run());


    [HttpGet]
    [Route("/Fan/Stop")]
    public IActionResult StopFan()
        => Ok(_fan.Stop());

    [HttpGet]
    [Route("/FogMachine/Run")]
    public IActionResult RunFogMachine()
        => Ok(_fm.Run());


    [HttpGet]
    [Route("/FogMachine/Stop")]
    public IActionResult StopFogMachine()
        => Ok(_fm.Stop());

    [HttpGet]
    public IActionResult Get()
    {
        var status = new RhizosphereStatus
        (
            FanRunning: _fan.IsRunning,
            FanUptime: _fan.Uptime,

            FogMachineRunning: _fm.IsRunning,
            FogMachineUptime: _fm.Uptime,
            
            Mode:"TODO",

            TemperatureCelsius: _cs.LatestTemperature?.Value.DegreesCelsius,
            LatestTemperatureRead: _cs.LatestTemperature?.Timestamp,

            HumidityPercentage: _cs.LatestRelativeHumidity?.Value.Value,
            LatestHumidityRead: _cs.LatestRelativeHumidity?.Timestamp
        );
        return Ok(status);
    }
}

public record RhizosphereStatus
(
    bool FanRunning,
    Uptime FanUptime,
    bool FogMachineRunning,
    Uptime FogMachineUptime,
    string Mode,
    double? TemperatureCelsius,
    DateTime? LatestTemperatureRead,
    double? HumidityPercentage,
    DateTime? LatestHumidityRead
);
