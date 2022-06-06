namespace Rhizosphere.Core;

public class UptimeLimit : Uptime
{
    public UptimeLimit() //better defaults
    {
        Daily = TimeSpan.FromDays(1);
        Hourly = TimeSpan.FromHours(1);
        Minutely = TimeSpan.FromMinutes(1);
    }
}

