using Microsoft.Extensions.Configuration;

namespace Rhizosphere.Core;


public class TimelapseService : BackgroundService
{
    private readonly ILogger<TimelapseService> _log;
    private readonly Webcam _cam;
    private readonly string _dir;

    public TimelapseService(ILogger<TimelapseService> logger, IConfiguration configuration,  Webcam webcam)
    {
        _log = logger;
        _cam = webcam;
        

        var directoryPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

        directoryPath = Path.Combine(directoryPath, "Timelapse");

        _dir = configuration.GetValue("TimelapseDirectory", directoryPath);
    }

    protected override async Task ExecuteAsync(CancellationToken token = default)
    {
        while (!token.IsCancellationRequested)
        {
            try
            {
                await Task.Delay(60000, token);
                await RunAsync(token);
            }
            catch (TaskCanceledException)
            {

            }
            catch (Exception ex)
            {
                _log.LogError("Severe error: {ex}", ex);
            }
        }

    }

    private async Task RunAsync(CancellationToken token = default)
    {
        if (!Directory.Exists(_dir))
            Directory.CreateDirectory(_dir);

        var fileName = $"{DateTime.Now.Year}_{DateTime.Now.Month}_{DateTime.Now.Day}_{DateTime.Now.Hour}.jpeg";

        var filePath = Path.Combine(_dir, fileName);

        if (File.Exists(filePath))
            return;

        await _cam.TakePhotoAsync(fileName, _dir, token);
    }
}