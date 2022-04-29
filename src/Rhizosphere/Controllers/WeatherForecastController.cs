using Microsoft.AspNetCore.Mvc;

namespace Rhizosphere.Controllers;

[ApiController]
[Route("[controller]")]
public class FanController : ControllerBase
{
    private readonly ILogger<FanController> _log;
    private readonly Fan _fan;

    public FanController(ILogger<FanController> logger, Fan fan)
    {
        _log = logger;
        _fan = fan;
    }

    [HttpGet]
    [Route("/Run")]
    public bool Run()
        => _fan.Run();
    

    [HttpGet]
    [Route("/Stop")]
    public bool Stop()
        => _fan.Run();
}
