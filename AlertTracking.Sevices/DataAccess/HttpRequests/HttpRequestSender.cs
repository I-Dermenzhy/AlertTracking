using AlertTracking.Abstractions.DataAccess.HttpRequests;

namespace AlertTracking.Services.DataAccess.HttpRequests;

public class HttpRequestSender : IHttpRequestSender
{
    private readonly IHttpClientFactory _clientFactory;

    public HttpRequestSender(IHttpClientFactory clientFactory) => _clientFactory = clientFactory;

    public async Task<HttpResponseMessage> SendHttpRequestAsync(HttpRequestMessage request, string? authorizationToken = null)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        var client = _clientFactory.CreateClient();

        if (authorizationToken is not null)
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authorizationToken);

        HttpResponseMessage response = await client.SendAsync(request);

        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"The HTTP request failed with status code {response.StatusCode}.");

        return response;
    }
}
