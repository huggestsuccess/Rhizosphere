namespace Rhizosphere.Core;

public class DeviceOptions
{
    public int PinNumber { get; set; }

    public bool NormallyOpen { get; set; } = false;

    public bool Enabled { get; set; } = true;

    public bool ActiveOverride { get; set; } = false;

    public UptimeLimit? UptimeLimit { get; set; }
}