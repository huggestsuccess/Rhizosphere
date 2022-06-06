namespace Rhizosphere.Core;

public class Phase
{
    public string Name { get; set; } = string.Empty;
    public float MinTemperature { get; set; } = 10;
    public float MaxTemperature { get; set; } = 30;
    public float MinHumidity { get; set; } = 50;
    public float MaxHumidity { get; set; } = 100;
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

public class RecipeExecution
{   
    public Recipe SelectedRecipe { get; set; }  = new();

    public DateTime StartedAt { get; set;} = DateTime.Now;
}
