using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Text.Json;

namespace Rhizosphere.Core;

public class RecipeRepository
{
    private readonly string _dirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Recipes");

    public IEnumerable<Recipe> GetRecipes()
    {
        if (!Directory.Exists(_dirPath))
            Directory.CreateDirectory(_dirPath);

        var dirContents = Directory.GetFiles(_dirPath, "*.json");

        foreach (var dirContent in dirContents)
        {
            var fileText = File.ReadAllText(dirContent);

            var recipe = JsonSerializer.Deserialize<Recipe>(fileText);

            if (recipe is null)
                continue;

            yield return recipe;
        }
    }

    public Recipe? GetRecipe(string recipeName)
     => GetRecipes().FirstOrDefault(r => r.Name == recipeName);
}