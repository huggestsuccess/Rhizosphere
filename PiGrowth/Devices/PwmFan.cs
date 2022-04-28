using System.Device.Gpio;
using System.Threading;

namespace PiGrowth
{
    public class PwmFan
    {
        private readonly GpioController _gpioCtrl;
        public int DutyCycle { get; set; }
        public PwmFan(GpioController c)
        {
            _gpioCtrl = c;
        }
        // public async Task RunAsync(CancellationToken token)
        // {
            
        // }

    }
}