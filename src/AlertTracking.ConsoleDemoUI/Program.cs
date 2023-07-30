using AlertTracking.ConsoleDemoUI;
using AlertTracking.Services;
using AlertTracking.Shared;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using IHost host = CreateHostBuilder(args).Build();
using IServiceScope scope = host.Services.CreateScope();

var services = scope.ServiceProvider;

IConfiguration configuration = services.GetRequiredService<IConfiguration>();

var app = services.GetRequiredService<App>();
await app.RunAsync();

static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureLogging(logging =>
        {
            logging.SetMinimumLevel(LogLevel.None);
        })
        .ConfigureAppConfiguration((hostingContext, config) =>
        {
            var env = hostingContext.HostingEnvironment;

            config
                .AddEnvironmentVariables()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddSharedConfiguration();

        })
        .ConfigureServices((hostContext, services) =>
        {
            services
                .AddAlertTracking()
                .AddSingleton(i => hostContext.Configuration)
                .AddTransient<App>();
        });
