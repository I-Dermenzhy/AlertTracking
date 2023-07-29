using AlertTracking.Domain.Dtos;

namespace AlertTracking.Abstractions.Configuration;

public interface IApiConfigurationProvider
{
    string GetApiAuthorizationToken();
    Endpoints GetApiEndpoints();
    string GetApiUrl();

    Dictionary<string, string> GetRegionNameIdPairs();
    string GetRegionId(string regionName);
    IEnumerable<string> GetRegionNames();
}
