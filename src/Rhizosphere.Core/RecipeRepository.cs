using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Hosting;


namespace Rhizosphere.Core;

public class RecipeRepository
{
    private readonly IFileProvider _fp;

    public RecipeRepository(IWebHostEnvironment environment)
    {
        _fp = environment.WebRootFileProvider;
    }

    public IEnumerable<Recipe> GetRecipes()
    {
        var dirContents = _fp.GetDirectoryContents("Recipes");
        foreach(var dirContent in dirContents)
        {
            System.Console.WriteLine(dirContent);
        }
        return Array.Empty<Recipe>();
    }
}