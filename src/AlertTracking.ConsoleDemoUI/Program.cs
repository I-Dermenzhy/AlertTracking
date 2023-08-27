﻿using AlertTracking.ConsoleDemoUI;
using AlertTracking.Services;
using AlertTracking.Shared;

using Microsoft.Extensions.Azure;
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
                .AddSharedConfiguration();

        })
        .ConfigureServices((hostContext, services) =>
        {
            services
                .AddAlertTracking()
                .AddSingleton(i => hostContext.Configuration)
                .AddTransient<App>();

            services.AddAzureClients(ac =>
            {
                ac.AddSecretClient(
                    hostContext.Configuration.GetSection("AzureKeyVaults"));
            });
        });
