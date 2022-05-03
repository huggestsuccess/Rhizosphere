using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Text.Json;

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
        foreach (var dirContent in dirContents)
        {
            if (dirContent.IsDirectory)
                continue;

            var fileText = File.ReadAllText(dirContent.PhysicalPath);

            var recipe = JsonSerializer.Deserialize<Recipe>(fileText);

            if (recipe is null)
                continue;

            yield return recipe;
        }
    }

    
}