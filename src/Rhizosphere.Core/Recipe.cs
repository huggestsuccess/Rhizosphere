namespace Rhizosphere.Core;

public class Phase
{
    public string Name { get; set; } = string.Empty;
    public int MinTemperature { get; set; }
    public int MaxTemperature { get; set; }
    public int MinHumidity { get; set; }
    public int MaxHumidity { get; set; }
    public string? Duration { get; set; }
    public string? CO2 { get; set; }
    public string? Light { get; set; }
}

public class Recipe
{
    public string Name { get; set; } = string.Empty;
    public string CommonName { get; set; } = string.Empty;
    public IEnumerable<Phase> Phases { get; set; } = Array.Empty<Phase>();
}

