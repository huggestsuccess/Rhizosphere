namespace Rhizosphere.Controllers;

[ApiController]
[Route("[controller]")]
public class RecipeController : ControllerBase
{
    private readonly ILogger<RecipeController> _log;
    private readonly RecipeRepository _rr;
    

    public RecipeController(ILogger<RecipeController> logger, RecipeRepository recipeRepository)
    {
        _log = logger;
        _rr = recipeRepository;
    }


    [HttpGet]
    public IActionResult Get()
    {  
        var r = _rr.GetRecipes();
        return Ok(r);
    }
}
