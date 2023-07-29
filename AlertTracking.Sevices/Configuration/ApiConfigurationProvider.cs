using AlertTracking.Abstractions.Configuration;
using AlertTracking.Domain.Dtos;

using Microsoft.Extensions.Configuration;

namespace AlertTracking.Services.Configuration;

public class ApiConfigurationProvider : IApiConfigurationProvider
{
    private readonly IConfiguration _configuration;

    public ApiConfigurationProvider(IConfiguration configuration) =>
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

    public string GetApiAuthorizationToken() => _configuration["AUTHORIZATION_TOKEN"] ??
        throw new InvalidOperationException("'AUTHORIZATION_TOKEN' is missed in the environment variables");

    public Endpoints GetApiEndpoints()
    {
        var endpoints = _configuration.GetSection("UkraineAlertApi:Endpoints").Get<Endpoints>();
        return endpoints ?? throw new InvalidOperationException("UkraineAlertApi:Endpoints configuration is missing or invalid.");
    }

    public string GetApiUrl()
    {
        return _configuration.GetValue<string>("UkraineAlertApi:Url") ??
            throw new InvalidOperationException("UkraineAlertApi:Url configuration is missing or invalid.");
    }

    public Dictionary<string, string> GetRegionNameIdPairs()
    {
        var regionIdsSection = _configuration.GetSection("UkraineAlertApi:RegionIds");

        if (!regionIdsSection.GetChildren().Any())
            throw new InvalidOperationException("UkraineAlertApi:RegionIds configuration is missing or invalid.");

        var regionIds = new Dictionary<string, string>();

        foreach (var regionId in regionIdsSection.GetChildren())
        {
            var regionName = regionId.Key;
            var regionIdValue = regionId.Value;

            if (string.IsNullOrEmpty(regionIdValue))
                throw new InvalidOperationException($"Region ID value is missing or invalid for region: {regionName}");

            regionIds.Add(regionName, regionIdValue);
        }

        return regionIds;
    }

    public string GetRegionId(string regionName)
    {
        Dictionary<string, string> regionIds = GetRegionNameIdPairs();

        if (!regionIds.ContainsKey(regionName))
            throw new InvalidOperationException($"Region ID value is missing or invalid for region: {regionName}");

        return regionIds[regionName];
    }

    public IEnumerable<string> GetRegionNames()
    {
        Dictionary<string, string> regionIds = GetRegionNameIdPairs();

        return regionIds.Keys;
    }
}
