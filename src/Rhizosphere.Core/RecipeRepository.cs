using Microsoft.Extensions.FileProviders;

namespace Rhizosphere.Core;

public class RecipeRepository
{
    private readonly IFileProvider _fp;

    public RecipeRepository(IFileProvider fileProvider)
    {
        _fp = fileProvider;
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