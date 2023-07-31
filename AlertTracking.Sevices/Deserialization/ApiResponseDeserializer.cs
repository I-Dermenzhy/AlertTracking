using AlertTracking.Abstractions.Deserialization;
using AlertTracking.Domain.Models;

using System.Text.Json;

namespace AlertTracking.Sevices.Deserialization;

public class ApiResponseDeserializer : IApiResponseDeserializer
{
    public async Task<Region> DeserializeRegionFromResponseAsync(HttpResponseMessage response)
    {
        var regions = await DeserializeRegionsFromResponseAsync(response);

        return regions?.FirstOrDefault() ??
            throw new HttpRequestException("An error occurred during the HTTP request");
    }

    public async Task<IEnumerable<Region>> DeserializeRegionsFromResponseAsync(HttpResponseMessage response)
    {
        using var responseStream = await response.Content.ReadAsStreamAsync();

        return await JsonSerializer.DeserializeAsync<IEnumerable<Region>>(responseStream) ??
            throw new HttpRequestException("An error occurred during the HTTP request");
    }

    public async Task<IEnumerable<Region>> DeserializeRegionsFromStatesResponseAsync(HttpResponseMessage response)
    {
        using var responseStream = await response.Content.ReadAsStreamAsync();

        // Api returns a repsonse which consists of a single object called 'States',
        // which contains an array of regions as its only attribute
        var statesObject = await JsonSerializer.DeserializeAsync<States>(responseStream) ??
            throw new HttpRequestException("An error occurred during the HTTP request");

        // A repsonse also contains a test region with an id = "0",
        // which has to be removed
        return statesObject.Regions.Where(r => r.Id != "0");
    }

    public async Task<long> DeserializeLastActionIndexFromResponseAsync(HttpResponseMessage response)
    {
        using var responseStream = await response.Content.ReadAsStreamAsync();
        var jsonDocument = await JsonDocument.ParseAsync(responseStream);

        return jsonDocument.RootElement.GetProperty("lastActionIndex").GetInt64();
    }
}
