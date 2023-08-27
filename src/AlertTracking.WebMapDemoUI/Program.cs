using AlertTracking.Services;
using AlertTracking.Shared;
using AlertTracking.WebMapDemoUI.Extensions;
using AlertTracking.WebMapDemoUI.Hubs;

using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args)
    .AddSharedConfiguration();

ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();

Configure(app, logger);

if (app.Environment.IsDevelopment())
    ConfigureForDevelopment(app, logger);

app.Run();

static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    var signalRConnectionString = "Endpoint=https://alert-tracking-signalr.service.signalr.net;AccessKey=u8/c5NEGufyMfnKYz5/kBiG56pnMl4bS5hG50EBpyv4=;Version=1.0;";

    services.AddSignalR().AddAzureSignalR(signalRConnectionString);

    services.AddAlertTracking();

    services.AddLogging(builder =>
    {
        builder.SetMinimumLevel(LogLevel.Information);
        builder.AddConsole();
    });

    services.AddAzureClients(ac =>
    {
        ac.AddSecretClient(
            configuration.GetSection("AzureKeyVaults"));
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