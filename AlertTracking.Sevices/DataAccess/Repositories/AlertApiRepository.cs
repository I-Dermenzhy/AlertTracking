using AlertTracking.Abstractions.DataAccess.HttpRequests;
using AlertTracking.Abstractions.DataAccess.Repositories;
using AlertTracking.Domain.Models;

using System.Text.Json;

namespace AlertTracking.Services.DataAccess.Repositories;

public class AlertApiRepository : IAlertApiRepository
{
    private readonly IApiRequestProvider _requestProvider;
    private readonly IHttpRequestSender _sender;

    private readonly string _authorizationToken;

    public AlertApiRepository(IApiRequestProvider requestProvider, IHttpRequestSender sender)
    {
        _requestProvider = requestProvider ?? throw new ArgumentNullException(nameof(requestProvider));
        _sender = sender ?? throw new ArgumentNullException(nameof(sender));

        _authorizationToken = _requestProvider.AuthorizationToken;
    }

    public async Task<Region> GetRegionAsync(string regionName)
    {
        HttpRequestMessage request = _requestProvider.GetRegionAlertsRequest(regionName);
        HttpResponseMessage response = await SendHttpRequestAsync(request);

        response.EnsureSuccessStatusCode();

        return await DeserializeRegionFromResponseAsync(response);
    }

    public async Task<IEnumerable<Region>> GetAllRegionsAsync()
    {
        HttpRequestMessage request = _requestProvider.GetRegionsRequest();
        HttpResponseMessage response = await SendHttpRequestAsync(request);

        response.EnsureSuccessStatusCode();

        return await DeserializeRegionsFromStatesResponseAsync(response);
    }

    public async Task<IEnumerable<Region>> GetRegionsWithAlertAsync()
    {
        HttpRequestMessage request = _requestProvider.GetRegionsWithAlertRequest();
        HttpResponseMessage response = await SendHttpRequestAsync(request);

        response.EnsureSuccessStatusCode();

        return await DeserializeRegionsFromResponseAsync(response);
    }

    public async Task<long> GetLastActionIndexAsync()
    {
        HttpRequestMessage request = _requestProvider.GetStatusUpdatesRequest();
        HttpResponseMessage response = await SendHttpRequestAsync(request);

        response.EnsureSuccessStatusCode();

        return await DeserializeLastActionIndexFromResponseAsync(response);
    }

    private async Task<HttpResponseMessage> SendHttpRequestAsync(HttpRequestMessage request) =>
        await _sender.SendHttpRequestAsync(request, _authorizationToken);

    private static async Task<Region> DeserializeRegionFromResponseAsync(HttpResponseMessage response)
    {
        var regions = await DeserializeRegionsFromResponseAsync(response);

        return regions?.FirstOrDefault() ??
            throw new HttpRequestException("An error occurred during the HTTP request");
    }

    private static async Task<IEnumerable<Region>> DeserializeRegionsFromResponseAsync(HttpResponseMessage response)
    {
        using var responseStream = await response.Content.ReadAsStreamAsync();

        return await JsonSerializer.DeserializeAsync<IEnumerable<Region>>(responseStream) ??
            throw new HttpRequestException("An error occurred during the HTTP request");
    }

    private static async Task<IEnumerable<Region>> DeserializeRegionsFromStatesResponseAsync(HttpResponseMessage response)
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

    private static async Task<long> DeserializeLastActionIndexFromResponseAsync(HttpResponseMessage response)
    {
        using var responseStream = await response.Content.ReadAsStreamAsync();
        var jsonDocument = await JsonDocument.ParseAsync(responseStream);

        return jsonDocument.RootElement.GetProperty("lastActionIndex").GetInt64();
    }
}
