namespace Rhizosphere.Core;


public class TimelapseService : BackgroundService
{
    private readonly ILogger<TimelapseService> _log;
    private readonly Webcam _cam;

    public TimelapseService(ILogger<TimelapseService> logger, Webcam webcam)
    {
        _log = logger;
        _cam = webcam;
    }

    protected override async Task ExecuteAsync(CancellationToken token = default)
    {
        while (!token.IsCancellationRequested)
        {
            await Task.Delay(60000, token);

            var directoryPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            directoryPath = Path.Combine(directoryPath, "Timelapse");

            if(!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            var fileName = $"{DateTime.Now.Year}_{DateTime.Now.Month}_{DateTime.Now.Day}_{DateTime.Now.Hour}.jpeg";
            
            var filePath = Path.Combine(directoryPath, fileName);

            if(File.Exists(filePath))
                continue;
            
            await _cam.TakePhotoAsync(fileName, directoryPath, token);
        }
    }
}