namespace AlertTracking.Abstractions.DataAccess.HttpRequests;

/// <summary>
/// Interface for providing API request configurations.
/// </summary>
public interface IApiRequestProvider
{
    /// <summary>
    /// Gets the API authorization token used for making authenticated requests.
    /// </summary>
    string AuthorizationToken { get; }

    /// <summary>
    /// Generates an HTTP request message to retrieve alerts for a specific region.
    /// </summary>
    /// <param name="regionName">The name of the region for which alerts are requested.</param>
    /// <returns>An instance of <see cref="HttpRequestMessage"/> representing the request configuration.</returns>
    HttpRequestMessage GetRegionAlertsRequest(string regionName);

    /// <summary>
    /// Generates an HTTP request message to retrieve all regions.
    /// </summary>
    /// <returns>An instance of <see cref="HttpRequestMessage"/> representing the request configuration.</returns>
    HttpRequestMessage GetRegionsRequest();

    /// <summary>
    /// Generates an HTTP request message to retrieve regions with active alerts.
    /// </summary>
    /// <returns>An instance of <see cref="HttpRequestMessage"/> representing the request configuration.</returns>
    HttpRequestMessage GetRegionsWithAlertRequest();

    /// <summary>
    /// Generates an HTTP request message to retrieve status updates.
    /// </summary>
    /// <returns>An instance of <see cref="HttpRequestMessage"/> representing the request configuration.</returns>
    HttpRequestMessage GetStatusUpdatesRequest();
}