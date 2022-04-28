using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Device.Gpio;
using Iot.Device.DHTxx;

namespace PiGrowth
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
        //dotnet publish --configuration Release -r linux-arm
        //put -r /Users/n/Development/PersonalMushroomComputer/PiGrowth/bin/Release/netcoreapp3.1/linux-arm/publish /home/pi/Development
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();

                });
    }
}
