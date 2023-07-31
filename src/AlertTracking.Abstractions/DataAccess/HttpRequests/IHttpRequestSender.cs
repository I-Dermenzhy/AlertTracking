namespace AlertTracking.Abstractions.DataAccess.HttpRequests;

/// <summary>
/// Defines an interface for sending HTTP requests and receiving corresponding HTTP responses asynchronously.
/// </summary>
public interface IHttpRequestSender
{
    /// <summary>
    /// Sends an HTTP request asynchronously and returns the corresponding HTTP response.
    /// </summary>
    /// <param name="request">The HTTP request message to be sent.</param>
    /// <param name="authorizationToken">An optional API authorization token to be included in the request headers for authenticated requests.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation, 
    /// yielding the <see cref="HttpResponseMessage"/> with the response from the server.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if the passed request is null.
    /// </exception>
    /// <exception cref="HttpRequestException">
    /// Thrown when http request sending is failed.
    /// </exception>
    Task<HttpResponseMessage> SendHttpRequestAsync(HttpRequestMessage request, string authorizationToken);
}