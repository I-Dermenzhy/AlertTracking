using AlertTracking.Abstractions.Configuration;
using AlertTracking.Abstractions.DataAccess.HttpRequests;

namespace AlertTracking.Services.DataAccess.HttpRequests;

public class ApiRequestProvider : IApiRequestProvider
{
    private readonly IApiConfigurationProvider _configurationProvider;
    private readonly string _url;

    public string AuthorizationToken => _configurationProvider.GetApiAuthorizationToken();

    public ApiRequestProvider(IApiConfigurationProvider configurationProvider)
    {
        _configurationProvider = configurationProvider ?? throw new ArgumentNullException(nameof(configurationProvider));
        _url = _configurationProvider.GetApiUrl();
    }

    public HttpRequestMessage GetRegionAlertsRequest(string regionName)
    {
        if (string.IsNullOrWhiteSpace(regionName))
            throw new ArgumentException($"'{nameof(regionName)}' cannot be null or whitespace.", nameof(regionName));

        string endpoint = _configurationProvider.GetApiEndpoints().RegionsWithAlerts;
        string regionId = _configurationProvider.GetRegionId(regionName);

        string path = string.Concat(_url, endpoint, regionId);

        return new(HttpMethod.Get, path);
    }

    public HttpRequestMessage GetRegionsRequest()
    {
        string endpoint = _configurationProvider.GetApiEndpoints().Regions;

        string path = string.Concat(_url, endpoint);

        return new(HttpMethod.Get, path);
    }

    public HttpRequestMessage GetRegionsWithAlertRequest()
    {
        string endpoint = _configurationProvider.GetApiEndpoints().RegionsWithAlerts;

        string path = string.Concat(_url, endpoint);

        return new(HttpMethod.Get, path);
    }

    public HttpRequestMessage GetStatusUpdatesRequest()
    {
        string endpoint = _configurationProvider.GetApiEndpoints().Status;

        string path = string.Concat(_url, endpoint);

        return new(HttpMethod.Get, path);
    }
}
