using AlertTracking.Abstractions.Configuration;
using AlertTracking.Abstractions.DataAccess.AzureKeyVaults;
using AlertTracking.Abstractions.Exceptions;

using Microsoft.Extensions.Configuration;

namespace AlertTracking.Services.Configuration;

public class ApiConfigurationProvider : IApiConfigurationProvider
{
    private const string ApiAuthorizationTokenSecretName = "UkraineAlertAPIAuthorizationToken";
    private const string ApiEndpointsSectionName = "UkraineAlertApi:Endpoints";
    private const string ApiUrlSectionName = "UkraineAlertApi:Url";
    private const string ApiRegionIdsSectionName = "UkraineAlertApi:RegionIds";

    private readonly IConfiguration _configuration;
    private readonly IKeyVaultsManager _vaultsManager;

    private string? _authorizationToken;

    public ApiConfigurationProvider(IConfiguration configuration, IKeyVaultsManager vaultsManager)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _vaultsManager = vaultsManager;
    }

    public string GetApiAuthorizationToken() => _authorizationToken ??=
        _vaultsManager.GetSecret(ApiAuthorizationTokenSecretName) ??
        throw new MissedKeyVaultSecretException(_vaultsManager, ApiAuthorizationTokenSecretName);

    public Endpoints GetApiEndpoints()
    {
        var endpoints = _configuration.GetSection(ApiEndpointsSectionName).Get<Endpoints>();

        return endpoints ?? throw new MissedConfigurationException(_configuration, ApiEndpointsSectionName);
    }

    public string GetApiUrl()
    {
        return _configuration.GetValue<string>(ApiUrlSectionName) ??
            throw new MissedConfigurationException(_configuration, ApiUrlSectionName);
    }

    public Dictionary<string, string> GetRegionNameIdPairs()
    {
        var regionIdsSection = _configuration.GetSection(ApiRegionIdsSectionName);

        if (!regionIdsSection.GetChildren().Any())
            throw new MissedConfigurationException(_configuration, ApiRegionIdsSectionName);

        var regionIds = new Dictionary<string, string>();

        foreach (var regionId in regionIdsSection.GetChildren())
        {
            var regionName = regionId.Key;
            var regionIdValue = regionId.Value;

            if (string.IsNullOrEmpty(regionIdValue))
                throw new MissedConfigurationException(_configuration, $"{ApiRegionIdsSectionName}:{regionName}");

            regionIds.Add(regionName, regionIdValue);
        }

        return regionIds;
    }

    public string GetRegionId(string regionName)
    {
        var regionIdsSection = _configuration.GetSection(ApiRegionIdsSectionName);

        return regionIdsSection.GetValue<string>(regionName) ??
            throw new MissedConfigurationException(_configuration, $"{ApiRegionIdsSectionName}:{regionName}");
    }

    public IEnumerable<string> GetRegionNames()
    {
        Dictionary<string, string> regionIds = GetRegionNameIdPairs();

        return regionIds.Keys;
    }
}
