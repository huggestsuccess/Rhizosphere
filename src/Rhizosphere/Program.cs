global using Rhizosphere.Core;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<Fan>();


builder.Services.AddSingleton<RecipeRepository>();
builder.Services.AddSingleton<ClimateService>();
builder.Services.AddHostedService(s=> s.GetRequiredService<ClimateService>());
builder.Services.AddHostedService<RhizosphereController>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseStaticFiles();

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
