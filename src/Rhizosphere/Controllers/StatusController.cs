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
    private readonly RhizosphereState _rs;

    public StatusController
        (
            ILogger<StatusController> logger, 
            IOptionsMonitor<RhizosphereOptions> optionsMonitor,  
            Fan fan, 
            ClimateService climateService, 
            FogMachine fogMachine,
            RhizosphereState rhizosphereState
        )
    {
        _log = logger;
        _fan = fan;
        _cs = climateService;
        _fm = fogMachine;
        _om = optionsMonitor;
        _rs = rhizosphereState;
    }
    [HttpGet]
    [Route("/Automatic")]
    public IActionResult SetAutomaticMode()
    {
        _rs.SetAutomatic();
        return Ok();
    }

    [HttpGet]
    [Route("/Manual")]
    public IActionResult SetManualMode()
    {
        _rs.SetManual();
        return Ok();
    }

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

            TemperatureCelsius: _cs.LatestTemperature?.Value.DegreesCelsius,
            LatestTemperatureRead: _cs.LatestTemperature?.Timestamp,

            HumidityPercentage: _cs.LatestRelativeHumidity?.Value.Value,
            LatestHumidityRead: _cs.LatestRelativeHumidity?.Timestamp,
            
            Mode: _rs.ManualMode ? "Manual" : "Automatic",
            ActiveRecipe: _rs.ActiveRecipe,
            ActivePhase: _rs.ActivePhase
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
    double? TemperatureCelsius,
    DateTime? LatestTemperatureRead,
    double? HumidityPercentage,
    DateTime? LatestHumidityRead,
    string Mode,
    Recipe ActiveRecipe,
    Phase ActivePhase
);
