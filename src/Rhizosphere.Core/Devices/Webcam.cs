using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Rhizosphere.Core;
public class Webcam
{
    private readonly ILogger<Webcam> _log;


    public Webcam(ILogger<Webcam> logger)
    {
        _log = logger;
    }

    public async Task TakePhotoAsync(string fileName, string directory, CancellationToken token = default)
    {
        _log.LogInformation("Taking a nice pic... ");

        using var process = new Process();
        process.StartInfo = new ProcessStartInfo
        {
            FileName = "/usr/bin/fswebcam",
            Arguments = $"--save {fileName} -S 6 -D 1 --no-banner --no-overlay --no-underlay --no-info --no-timestamp --no-subtitle -r 1920x1080",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            WorkingDirectory = directory
        };

        process.Start();
        string result = await process.StandardOutput.ReadToEndAsync();

        _log.LogInformation(result);

        await process.WaitForExitAsync(token);
    }

    public async Task<FileStream> GetPhotoFileStream(CancellationToken token = default)
    {
        var fileName = "webCamShot.jpeg";

        await TakePhotoAsync(fileName, AppDomain.CurrentDomain.BaseDirectory, token);
        return File.OpenRead("image.jpeg");
    }
}
