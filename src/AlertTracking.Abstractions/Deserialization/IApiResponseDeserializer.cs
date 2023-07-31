using AlertTracking.Domain.Models;

namespace AlertTracking.Abstractions.Deserialization;

public interface IApiResponseDeserializer
{
    Task<long> DeserializeLastActionIndexFromResponseAsync(HttpResponseMessage response);
    Task<Region> DeserializeRegionFromResponseAsync(HttpResponseMessage response);
    Task<IEnumerable<Region>> DeserializeRegionsFromResponseAsync(HttpResponseMessage response);
    Task<IEnumerable<Region>> DeserializeRegionsFromStatesResponseAsync(HttpResponseMessage response);
}