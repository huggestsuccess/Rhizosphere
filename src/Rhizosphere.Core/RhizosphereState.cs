namespace Rhizosphere.Core;

public class RhizosphereState
{
    private readonly ILogger<RhizosphereState> _log;
    private readonly IOptionsMonitor<RhizosphereOptions> _om;
    private readonly RecipeRepository _rr;

    public RhizosphereState(ILogger<RhizosphereState> logger,  IOptionsMonitor<RhizosphereOptions> options, RecipeRepository recipeRepository)
    {
        _log = logger;
        _om  = options;
        _rr = recipeRepository;

        var recipe = GetActiveRecipe(_om.CurrentValue.ActiveRecipeName);
        var phase = GetActivePhase(recipe, _om.CurrentValue.ActivePhaseName);
        ActivePhase = phase ?? new();
        ActiveRecipe = recipe ?? new();

        ManualMode = _om.CurrentValue.ManualMode;
    }

    public bool ManualMode { get; private set; } = false;
    
    public Phase ActivePhase { get; private set; } = new();

    public Recipe ActiveRecipe { get; private set; } = new();

    public void SetManual()
        => ManualMode = true;

    public void SetAutomatic()
        => ManualMode = false;

    private Recipe? GetActiveRecipe(string activeRecipeName)
        => _rr.GetRecipe(activeRecipeName);

    private Phase? GetActivePhase(Recipe? r, string activePhaseName)
    {
        if (r is null)
            return null;

        return r.Phases.FirstOrDefault(f => f.Name == activePhaseName);
    }

}