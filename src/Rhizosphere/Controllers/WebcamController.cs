using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
namespace Rhizosphere.Controllers;

[ApiController]
[Route("[controller]")]
public class WebcamController : ControllerBase
{

    private readonly ILogger<WebcamController> _logger;
    private readonly Webcam _cam;

    public WebcamController(ILogger<WebcamController> logger, Webcam cam)
    {
        _logger = logger;
        _cam = cam;
    }

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken token = default)
    {
        FileStream stream = await _cam.GetPhotoFileStream(token);

        return File(stream, "image/jpeg");
    }
}
