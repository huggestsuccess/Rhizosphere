namespace Rhizosphere.Core;

public class Uptime
{
    public TimeSpan Daily { get; set; }
    public TimeSpan Hourly { get; set; }
    public TimeSpan Minutely { get; set; }

    public Uptime()
    {

    }

    public Uptime(Uptime previous, DateTime from, DateTime to, bool isRunning)
    {
        if (isRunning)
        {
            var dt = to - from;

            if (from.Minute == to.Minute)
                Minutely += dt;
            else
                Minutely = TimeSpan.FromSeconds(dt.Seconds);

            if (from.Hour == to.Hour)
                Hourly += dt;
            else
                Hourly = TimeSpan.FromSeconds(dt.Minutes);

            if (from.Day == to.Day)
                Daily += dt;
            else
                Daily = TimeSpan.FromSeconds(dt.Seconds);
        }
        else
        {
            if (from.Minute == to.Minute)
                Minutely = previous.Minutely;
            else
                Minutely = default;

            if (from.Hour == to.Hour)
                Hourly = previous.Minutely;
            else
                Hourly = default;

            if (from.Day == to.Day)
                Daily = previous.Daily;
            else
                Daily = default;
        }
    }


    public override string? ToString()
        => $"D{Daily}|h{Hourly}|m{Minutely}";
}
