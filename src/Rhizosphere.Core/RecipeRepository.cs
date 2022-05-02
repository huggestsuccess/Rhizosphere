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
        foreach(var dirContent in dirContents)
        {
            var fileText = File.ReadAllText(dircontent.FullPath);
        }
        return Array.Empty<Recipe>();
    }
}