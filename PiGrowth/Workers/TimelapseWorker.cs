using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;

namespace PiGrowth
{
    public class TimelapseWorker : BackgroundService
    {        
        private int _photoCount = 0;  
        private readonly WebCam _cam;
        private readonly ILogger<TimelapseWorker> _log;
        private readonly DirectoryInfo _outputDirecory;

        public TimelapseWorker(ILogger<TimelapseWorker> logger, WebCam cam, IHostEnvironment env)
        {
            _cam = cam;
            _log = logger;

            _outputDirecory = new DirectoryInfo(Path.Combine(env.ContentRootPath,"Timelapses"));

            if (!_outputDirecory.Exists)
                _outputDirecory.Create();
        }

        protected override async Task ExecuteAsync(CancellationToken token = default)
        {
            while (!token.IsCancellationRequested)
            {
                _log.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                _photoCount++;
                string fileName = GetFileName();
                _log.LogInformation($"Getting a new image :{fileName}");

                using (var stream = await _cam.GetPhotoFileStream(token))
                {
                    
                    _log.LogInformation($"writing image to file :{fileName}");
                    await WritePhotoToFileAsync(stream, fileName,token);

                    _log.LogInformation($"Wrote file :{fileName}");
                }
                
                await Task.Delay(30000, token);
            }
        }
        private string GetFileName()
        => Path.Combine(_outputDirecory.FullName,$"{_photoCount}.jpeg");

        private async Task WritePhotoToFileAsync(FileStream stream, string destFileName,CancellationToken token= default)
        {
            using (FileStream newFile = File.Create(GetFileName()))
                await newFile.CopyToAsync(stream,token);

        }
    }
}
