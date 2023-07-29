namespace AlertTracking.Abstractions.DataAccess.HttpRequests;

public interface IHttpRequestSender
{
    Task<HttpResponseMessage> SendHttpRequestAsync(HttpRequestMessage request, string authorizationToken);
}