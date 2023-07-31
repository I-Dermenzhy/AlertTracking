using AlertTracking.Abstractions.Configuration;
using AlertTracking.Abstractions.DataAccess.HttpRequests;
using AlertTracking.Abstractions.DataAccess.Repositories;
using AlertTracking.Abstractions.Deserialization;
using AlertTracking.Abstractions.Monitors;
using AlertTracking.Services.Configuration;
using AlertTracking.Services.DataAccess.HttpRequests;
using AlertTracking.Services.DataAccess.Repositories;
using AlertTracking.Services.Monitors;
using AlertTracking.Sevices.Deserialization;

using Microsoft.Extensions.DependencyInjection;

namespace AlertTracking.Services;

public static class CompositionRoot
{
    public static IServiceCollection AddAlertTracking(this IServiceCollection services) =>
        services
            .AddHttpClient()
            .AddSingleton<IApiConfigurationProvider, ApiConfigurationProvider>()
            .AddTransient<IHttpRequestSender, HttpRequestSender>()
            .AddTransient<IApiRequestProvider, ApiRequestProvider>()
            .AddTransient<IApiResponseDeserializer, ApiResponseDeserializer>()
            .AddTransient<IAlertApiRepository, AlertApiRepository>()
            .AddTransient<IStatusUpdatesMonitor, StatusUpdatesMonitor>()
            .AddTransient<IRegionAlertMonitor, RegionAlertMonitor>();
}
