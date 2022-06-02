

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
        
        if (uptime.Minutely >= limit.Minutely)
            return false;

        if (uptime.Hourly >= limit.Hourly)
            return false;

        if (uptime.Daily >= limit.Daily)
            return false;
        
        
        return true;
    }
}