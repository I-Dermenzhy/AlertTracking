using AlertTracking.Abstractions.Deserialization;
using AlertTracking.Abstractions.Exceptions;
using AlertTracking.Domain.Models;

using System.Text.Json;

namespace AlertTracking.Sevices.Deserialization;

public sealed class ApiResponseDeserializer : IApiResponseDeserializer
{
    public async Task<Region> DeserializeRegionFromResponseAsync(HttpResponseMessage response)
    {
        ArgumentNullException.ThrowIfNull(response, nameof(response));

        var regions = await DeserializeRegionsFromResponseAsync(response);

        return regions.First();
    }

    public async Task<IEnumerable<Region>> DeserializeRegionsFromResponseAsync(HttpResponseMessage response)
    {
        ArgumentNullException.ThrowIfNull(response, nameof(response));

        using var responseStream = await response.Content.ReadAsStreamAsync();

        try
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<Region>>(responseStream) ??
                throw new HttpRequestException("The repsonse contains no instances of the type 'Region'");
        }
        catch (Exception ex) when (ex is not HttpRequestException)
        {
            throw new HttpResponseDeserializationException(response, ex);
        }
    }

    public async Task<IEnumerable<Region>> DeserializeRegionsFromStatesResponseAsync(HttpResponseMessage response)
    {
        ArgumentNullException.ThrowIfNull(response, nameof(response));

        using var responseStream = await response.Content.ReadAsStreamAsync();

        try
        {
            // Api returns a repsonse which consists of a single object called 'States',
            // which contains an array of regions as its only attribute
            var statesObject = await JsonSerializer.DeserializeAsync<States>(responseStream) ??
                throw new HttpRequestException("The repsonse contains no instances of the type 'States'");

            // A repsonse also contains a test region with an id = "0",
            // which has to be removed
            return statesObject.Regions.Where(r => r.Id != "0") ??
                 throw new HttpRequestException("The 'states' object contains no regions");
        }
        catch (Exception ex) when (ex is not HttpRequestException)
        {
            throw new HttpResponseDeserializationException(response, ex);
        }
    }

    public async Task<long> DeserializeLastActionIndexFromResponseAsync(HttpResponseMessage response)
    {
        ArgumentNullException.ThrowIfNull(response, nameof(response));

        using var responseStream = await response.Content.ReadAsStreamAsync();

        try
        {
            var jsonDocument = await JsonDocument.ParseAsync(responseStream);

            return jsonDocument.RootElement.GetProperty("lastActionIndex").GetInt64();
        }
        catch (Exception ex)
        {
            throw new HttpResponseDeserializationException(response, ex);
        }
    }
}
