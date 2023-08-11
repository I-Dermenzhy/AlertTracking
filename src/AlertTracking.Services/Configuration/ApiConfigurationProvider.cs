using AlertTracking.Abstractions.Configuration;
using AlertTracking.Abstractions.Exceptions;

using Microsoft.Extensions.Configuration;

namespace AlertTracking.Services.Configuration;

public class ApiConfigurationProvider : IApiConfigurationProvider
{
    private readonly IConfiguration _configuration;

    public ApiConfigurationProvider(IConfiguration configuration) =>
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

    public string GetApiAuthorizationToken() => _configuration["AUTHORIZATION_TOKEN"] ??
        throw new MissedConfigurationException(_configuration, "AUTHORIZATION_TOKEN");

    public Endpoints GetApiEndpoints()
    {
        var endpoints = _configuration.GetSection("UkraineAlertApi:Endpoints").Get<Endpoints>();
        return endpoints ?? throw new MissedConfigurationException(_configuration, "UkraineAlertApi:Endpoints");
    }

    public string GetApiUrl()
    {
        return _configuration.GetValue<string>("UkraineAlertApi:Url") ??
            throw new MissedConfigurationException(_configuration, "UkraineAlertApi:Url");
    }

    public Dictionary<string, string> GetRegionNameIdPairs()
    {
        var regionIdsSection = _configuration.GetSection("UkraineAlertApi:RegionIds");

        if (!regionIdsSection.GetChildren().Any())
            throw new MissedConfigurationException(_configuration, "UkraineAlertApi:RegionIds");

        var regionIds = new Dictionary<string, string>();

        foreach (var regionId in regionIdsSection.GetChildren())
        {
            var regionName = regionId.Key;
            var regionIdValue = regionId.Value;

            if (string.IsNullOrEmpty(regionIdValue))
                throw new MissedConfigurationException(_configuration, $"UkraineAlertApi:RegionIds:{regionName}");

            regionIds.Add(regionName, regionIdValue);
        }

        return regionIds;
    }

    public string GetRegionId(string regionName)
    {
        var regionIdsSection = _configuration.GetSection("UkraineAlertApi:RegionIds");

        return regionIdsSection.GetValue<string>(regionName) ??
            throw new MissedConfigurationException(_configuration, $"UkraineAlertApi:RegionIds:{regionName}");
    }

    public IEnumerable<string> GetRegionNames()
    {
        Dictionary<string, string> regionIds = GetRegionNameIdPairs();

        return regionIds.Keys;
    }
}
