namespace Rhizosphere.Core;


public class RhizosphereOptions
{
    public string ActiveRecipeName { get; set; } = string.Empty;
    public string ActivePhaseName { get; set; } = string.Empty;
    public bool ManualMode { get; set; } = false;
    public FanOptions FanOptions { get; set; } = new();
    public FogMachineOptions FogMachineOptions { get; set; } = new();
}