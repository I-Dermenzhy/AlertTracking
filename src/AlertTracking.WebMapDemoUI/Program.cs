using AlertTracking.Services;
using AlertTracking.Shared;
using AlertTracking.WebMapDemoUI.Extensions;
using AlertTracking.WebMapDemoUI.Hubs;

var builder = WebApplication.CreateBuilder(args)
    .AddSharedConfiguration();

ConfigureServices(builder.Services);

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();

Configure(app, logger);

if (app.Environment.IsDevelopment())
    ConfigureForDevelopment(app, logger);

app.Run();

static void ConfigureServices(IServiceCollection services)
{
    services.AddSignalR();

    services.AddAlertTracking();

    services.AddLogging(builder =>
    {
        builder.SetMinimumLevel(LogLevel.Information);
        builder.AddConsole();
    });
}

static void Configure(WebApplication app, ILogger<Program> logger)
{
    logger.LogInformation("Configuring the application.");

    app.UseDefaultFiles();
    app.UseStaticFiles();

    app.UseRouting();

    app.MapHub<AlertHub>("/alert");
}

static void ConfigureForDevelopment(WebApplication app, ILogger<Program> logger)
{
    logger.LogInformation("Configuring for development.");

    app.UseDeveloperExceptionPage();
}