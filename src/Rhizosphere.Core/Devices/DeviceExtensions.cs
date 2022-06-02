

using System;
using Microsoft.Extensions.Options;

namespace Rhizosphere.Core;

public static class DeviceExtensions
{
    public static bool IsWithinUptimeLimit(this Device device)
    {
        var limit = device.DeviceOptions.UptimeLimit;

        if (limit is null)
            return true;

        var uptime = device.Uptime;

        
        if (uptime.Minutely > limit.Minutely)
        {
            System.Console.WriteLine("Reached Minutely uptime limit");
            return false;
        }

        if (uptime.Hourly > limit.Hourly)
        {
            System.Console.WriteLine("Reached Minutely Hourly limit");
            return false;
        }

        if (uptime.Daily > limit.Daily)
        {
            System.Console.WriteLine("Reached Minutely Daily limit");
            return false;
        }
        System.Console.WriteLine("Device: {0}, Uptime: {1}, Limit: {2}", device, uptime, limit);

        return true;
    }
}