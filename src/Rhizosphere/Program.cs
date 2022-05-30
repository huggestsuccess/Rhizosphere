global using Rhizosphere.Core;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// builder.UseSystemd();
builder.Host.UseSystemd();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<Fan>();
builder.Services.AddSingleton<FogMachine>();


builder.Services.AddSingleton<RecipeRepository>();
builder.Services.AddSingleton<ClimateService>();
builder.Services.AddHostedService(s=> s.GetRequiredService<ClimateService>());
builder.Services.AddHostedService<RhizosphereHandler>();


builder.Services.Configure<RhizosphereOptions>(builder.Configuration.GetSection(nameof(RhizosphereOptions)));

builder.Services.Configure<FanOptions>(builder.Configuration.GetSection(nameof(RhizosphereOptions) + ":" + nameof(FanOptions)));
builder.Services.Configure<FogMachineOptions>(builder.Configuration.GetSection(nameof(RhizosphereOptions) + ":" + nameof(FogMachineOptions)));


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseStaticFiles();

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
