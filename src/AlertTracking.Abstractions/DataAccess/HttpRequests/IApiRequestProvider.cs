using AlertTracking.Abstractions.Exceptions;

namespace AlertTracking.Abstractions.DataAccess.HttpRequests;
/// <summary>
/// Interface for providing API request configurations.
/// </summary>
public interface IApiRequestProvider
{
    /// <summary>
    /// Gets the API authorization token used for making authenticated requests.
    /// </summary>
    /// <exception cref="MissedConfigurationException">
    /// Thrown if the API authorization token is missed in the configuration provider.
    /// </exception>
    string AuthorizationToken { get; }

    /// <summary>
    /// Generates an HTTP request message to retrieve alerts for a specific region.
    /// </summary>
    /// <param name="regionName">The name of the region for which alerts are requested.</param>
    /// <returns>An instance of <see cref="HttpRequestMessage"/> representing the request configuration.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown if the passed region name is null, empty, or whitespace.
    /// </exception>
    /// <exception cref="MissedConfigurationException">
    /// Thrown if the API region endpoint for the passed region is missed in the configuration provider.
    /// </exception>
    HttpRequestMessage GetRegionAlertsRequest(string regionName);

    /// <summary>
    /// Generates an HTTP request message to retrieve all regions.
    /// </summary>
    /// <returns>An instance of <see cref="HttpRequestMessage"/> representing the request configuration.</returns>
    /// <exception cref="MissedConfigurationException">
    /// Thrown if the API regions endpoint is missed in the configuration provider.
    /// </exception>
    HttpRequestMessage GetRegionsRequest();

    /// <summary>
    /// Generates an HTTP request message to retrieve regions with active alerts.
    /// </summary>
    /// <returns>An instance of <see cref="HttpRequestMessage"/> representing the request configuration.</returns>
    /// /// <exception cref="MissedConfigurationException">
    /// Thrown if the API regions with alerts endpoint is missed in the configuration provider.
    /// </exception>
    HttpRequestMessage GetRegionsWithAlertRequest();

    /// <summary>
    /// Generates an HTTP request message to retrieve status updates.
    /// </summary>
    /// <returns>An instance of <see cref="HttpRequestMessage"/> representing the request configuration.</returns>
    ///  /// /// <exception cref="MissedConfigurationException">
    /// Thrown if the API status endpoint is missed in the configuration provider.
    /// </exception>
    HttpRequestMessage GetStatusUpdatesRequest();
}