using System;
using System.Threading;
using System.Threading.Tasks;
using Iot.Device.DHTxx;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace PiGrowth
{
	internal class GpioWorker : BackgroundService
	{
		private readonly ILogger<GpioWorker> _log;
		private readonly Iot.Device.DHTxx.Dht22 _dht22;
		public GpioWorker(ILogger<GpioWorker> logger, Iot.Device.DHTxx.Dht22 dht22)
		{
			_log = logger;
			_dht22 = dht22;
		}

		protected override async Task ExecuteAsync(CancellationToken token = default)
		{
			double temperature, humidity;
			await Task.Yield();

			while (!token.IsCancellationRequested)
			{
				_log.LogInformation("Reading values");
				temperature = _dht22.Temperature.Celsius;
				humidity = _dht22.Humidity;

				_log.LogInformation($"Last read successful? {_dht22.IsLastReadSuccessful}");
				
				if(!double.IsNaN(temperature) && !double.IsNaN(humidity))
					_log.LogInformation($"Temperature :{temperature} C° Humidity: {humidity}%");

				await Task.Delay(5000, token);
			}
		}
	}
}