var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSystemd();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<Fan>();
builder.Services.AddSingleton<Webcam>();
builder.Services.AddSingleton<FogMachine>();

builder.Services.AddSingleton<RecipeRepository>();
builder.Services.AddSingleton<ClimateService>();
builder.Services.AddSingleton<RhizosphereState>();

builder.Services.AddHostedService(s => s.GetRequiredService<ClimateService>());
builder.Services.AddHostedService<RhizosphereHandler>();
builder.Services.AddHostedService<TimelapseService>();

builder.Services.Configure<RhizosphereOptions>(builder.Configuration.GetSection(nameof(RhizosphereOptions)));
builder.Services.Configure<FanOptions>(builder.Configuration.GetSection(nameof(RhizosphereOptions) + ":" + nameof(FanOptions)));
builder.Services.Configure<FogMachineOptions>(builder.Configuration.GetSection(nameof(RhizosphereOptions) + ":" + nameof(FogMachineOptions)));


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

try
{
    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}