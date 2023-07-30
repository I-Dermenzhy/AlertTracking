using AlertTracking.Abstractions.Exceptions;

namespace AlertTracking.Abstractions.Configuration;

/// <summary>
/// Represents a provider for API configuration settings.
/// </summary>
public interface IApiConfigurationProvider
{
    /// <summary>
    /// Gets an API authorization token from the available configuration sources.
    /// An API authorization token is required for making authenticated requests to the API.
    /// </summary>
    /// <returns>A <see cref="string"/> representing an API authorization token.</returns>
    /// <exception cref="MissedConfigurationException">
    /// Thrown if the API authorization token is not found in the configuration sources.
    /// </exception>
    string GetApiAuthorizationToken();

    /// <summary>
    /// Gets an API endpoint configurations from the available configuration sources.
    /// </summary>
    /// <returns>An <see cref="Endpoints"/> instance representing the API endpoints.</returns>
    /// <exception cref="MissedConfigurationException">
    /// Thrown if the API endpoint configurations are not found in the configuration sources.
    /// </exception>
    Endpoints GetApiEndpoints();

    /// <summary>
    /// Gets the base URL for the API from the available configuration sources.
    /// </summary>
    /// <returns>A <see cref="string"/> representing the base URL of the API.</returns>
    /// <exception cref="MissedConfigurationException">
    /// Thrown if the API base URL is not found in the configuration sources.
    /// </exception>
    string GetApiUrl();

    /// <summary>
    /// Gets the region ID for the specified region name from the available configuration sources.
    /// </summary>
    /// <param name="regionName">The name of the region.</param>
    /// <returns>A <see cref="string"/> representing the region ID.</returns>
    /// <exception cref="MissedConfigurationException">
    /// Thrown if the region name and ID pairs are not found in the configuration sources
    /// or the region ID value is missing or invalid for the specified region name.
    /// </exception>
    string GetRegionId(string regionName);

    /// <summary>
    /// Gets the names of all regions from the available configuration sources.
    /// </summary>
    /// <returns>An <see cref="IEnumerable{string}"/> containing the names of all regions.</returns>
    /// <exception cref="MissedConfigurationException">
    /// Thrown if the region names are not found in the configuration sources.
    /// </exception>
    IEnumerable<string> GetRegionNames();

    /// <summary>
    /// Gets a dictionary of region names and their corresponding IDs from the available configuration sources.
    /// </summary>
    /// <returns>
    /// A <see cref="Dictionary{string, string}"/> containing region names as keys and region IDs as values.
    /// </returns>
    /// <exception cref="MissedConfigurationException">
    /// Thrown if the region name and ID pairs are not found in the configuration sources
    /// or the region ID value is missing or invalid for a specific region.
    /// </exception>
    Dictionary<string, string> GetRegionNameIdPairs();
}

/// <summary>
/// Represents a configuration record that holds the relative endpoints for different API functionalities.
/// </summary>
/// <remarks>
/// The <see cref="Endpoints"/> record is used to define the relative endpoints for specific actions in the API.
/// Each endpoint is appended to the base URL of the API to form complete API URLs for making requests.
/// </remarks>
public record Endpoints(
    /// <summary>
    /// Gets the relative endpoint for retrieving regions with alerts.
    /// </summary>
    string RegionsWithAlerts,

    /// <summary>
    /// Gets the relative endpoint for retrieving region history.
    /// </summary>
    string RegionHistory,

    /// <summary>
    /// Gets the relative endpoint for retrieving status information.
    /// </summary>
    string Status,

    /// <summary>
    /// Gets the relative endpoint for retrieving regions information.
    /// </summary>
    string Regions,

    /// <summary>
    /// Gets the relative endpoint for webhook integration.
    /// </summary>
    string Webhook
);
