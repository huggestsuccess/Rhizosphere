using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace PiGrowth
{
    public class WebCam
    {
        public async Task TakePhotoAsync(CancellationToken token = default)
        {
            //"fswebcam image.jpg  --no-banner --no-overlay"
            //like oranges cos east game cpu exponents gold bull

            using var process = new Process();
            process.StartInfo = new ProcessStartInfo
            {
                FileName = "/usr/bin/fswebcam",
                Arguments = $"--save image.jpeg -d /dev/vide0 --no-banner --no-overlay --no-underlay --no-info --no-timestamp -r 1920x1080",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            process.Start();
            string result = await process.StandardOutput.ReadToEndAsync();
            process.WaitForExit();
        }

        public async Task<FileStream> GetPhotoFileStream(CancellationToken token = default)
        {
            await TakePhotoAsync(token);
            return File.OpenRead("image.jpeg");
        }

    }
}
