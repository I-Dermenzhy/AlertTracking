using AlertTracking.Abstractions.DataAccess.HttpRequests;
using AlertTracking.Abstractions.DataAccess.Repositories;
using AlertTracking.Abstractions.Deserialization;
using AlertTracking.Domain.Models;

namespace AlertTracking.Services.DataAccess.Repositories;

public class AlertApiRepository : IAlertApiRepository
{
    private readonly IApiRequestProvider _requestProvider;
    private readonly IApiResponseDeserializer _responseDeserializer;
    private readonly IHttpRequestSender _sender;

    private readonly string _authorizationToken;

    public AlertApiRepository(IApiRequestProvider requestProvider, IApiResponseDeserializer responseDeserializer, IHttpRequestSender sender)
    {
        _requestProvider = requestProvider ?? throw new ArgumentNullException(nameof(requestProvider));
        _responseDeserializer = responseDeserializer ?? throw new ArgumentNullException(nameof(responseDeserializer));
        _sender = sender ?? throw new ArgumentNullException(nameof(sender));

        _authorizationToken = _requestProvider.AuthorizationToken;
    }

    public async Task<Region> GetRegionAsync(string regionName)
    {
        if (string.IsNullOrWhiteSpace(regionName))
            throw new ArgumentException($"'{nameof(regionName)}' cannot be null or whitespace.", nameof(regionName));

        HttpRequestMessage request = _requestProvider.GetRegionAlertsRequest(regionName);
        HttpResponseMessage response = await SendHttpRequestAsync(request);

        response.EnsureSuccessStatusCode();

        return await _responseDeserializer.DeserializeRegionFromResponseAsync(response);
    }

    public async Task<IEnumerable<Region>> GetAllRegionsAsync()
    {
        HttpRequestMessage request = _requestProvider.GetRegionsRequest();
        HttpResponseMessage response = await SendHttpRequestAsync(request);

        response.EnsureSuccessStatusCode();

        return await _responseDeserializer.DeserializeRegionsFromStatesResponseAsync(response);
    }

    public async Task<IEnumerable<Region>> GetRegionsWithAlertAsync()
    {
        HttpRequestMessage request = _requestProvider.GetRegionsWithAlertRequest();
        HttpResponseMessage response = await SendHttpRequestAsync(request);

        response.EnsureSuccessStatusCode();

        return await _responseDeserializer.DeserializeRegionsFromResponseAsync(response);
    }

    public async Task<long> GetLastActionIndexAsync()
    {
        HttpRequestMessage request = _requestProvider.GetStatusUpdatesRequest();
        HttpResponseMessage response = await SendHttpRequestAsync(request);

        response.EnsureSuccessStatusCode();

        return await _responseDeserializer.DeserializeLastActionIndexFromResponseAsync(response);
    }

    private async Task<HttpResponseMessage> SendHttpRequestAsync(HttpRequestMessage request) =>
        await _sender.SendHttpRequestAsync(request, _authorizationToken);
}
