using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using Iot.Device.DHTxx;

namespace PiGrowth.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatusController : ControllerBase
    {
        public class BiomeStatusResponse
        {
            public double TemperatureCelsius { get; set; }
            public int HumidityPercentage { get; set; }
            public int Co2Percentage { get; set; }
        }

        private readonly ILogger<StatusController> _log;
        private readonly Dht22 _dht22;

        public StatusController(ILogger<StatusController> logger, Dht22 dht22)
        {
            _log = logger;
            _dht22 = dht22;
        }

        [HttpGet]
        public async Task<BiomeStatusResponse> Get(CancellationToken token = default)
        {
            await Task.Yield();
            while(!token.IsCancellationRequested)
                if(_dht22.IsLastReadSuccessful)
                {
                    double temperature, humidity;
                    temperature = _dht22.Temperature.Celsius;
				    humidity = _dht22.Humidity;
                    if(!double.IsNaN(temperature) && !double.IsNaN(humidity))
                        return new BiomeStatusResponse
                        {
                            TemperatureCelsius = Math.Round(temperature,2),
                            HumidityPercentage = (int) humidity,
                            Co2Percentage = 0
                        };
                }
            _log.LogError($"Cancellation was requested for GetStatus");
            return default;
        }
    }
}
