using AlertTracking.Services;
using AlertTracking.Shared;
using AlertTracking.WebMapDemoUI.Extensions;
using AlertTracking.WebMapDemoUI.Hubs;

var builder = WebApplication.CreateBuilder(args)
    .AddSharedConfiguration();

ConfigureServices(builder.Services);

var app = builder.Build();

Configure(app);

if (app.Environment.IsDevelopment())
    ConfigureForDevelopment(app);

app.Run();

static void ConfigureServices(IServiceCollection services)
{
    services.AddSignalR();

    services.AddAlertTracking();
}

static void Configure(WebApplication app)
{
    app.UseDefaultFiles();
    app.UseStaticFiles();

    app.UseRouting();

    app.MapHub<AlertHub>("/alert");
}

static void ConfigureForDevelopment(WebApplication app)
{
    app.UseDeveloperExceptionPage();
}