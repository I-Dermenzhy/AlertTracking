using AlertTracking.Abstractions.Exceptions;
using AlertTracking.Domain.Models;

namespace AlertTracking.Abstractions.DataAccess.Repositories;

/// <summary>
/// Represents a repository for retrieving alert-related data from the Web API (https://api.ukrainealarm.com/).
/// </summary>
public interface IAlertApiRepository
{
    /// <summary>
    /// Gets the last action index indicating whether the API data has changed. 
    /// The last action index is a unique number generated for each change in the API data, allowing clients to track updates.
    /// </summary>
    /// <returns>The last action index as a <see cref="long"/> value.</returns>
    /// <exception cref="HttpRequestException">Thrown when an error occurs during the HTTP request or the response contains no instances of the type 'lastActionIndex'.</exception>
    Task<long> GetLastActionIndexAsync();

    /// <summary>
    /// Gets a region with its alert status.
    /// </summary>
    /// <param name="regionName">The name of the region to retrieve.</param>
    /// <returns>
    /// The <see cref="Region"/> object representing the region with its alert status.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when the provided regionName is null or whitespace.</exception>
    /// <exception cref="HttpRequestException">Thrown when an error occurs during the HTTP request or the response contains no instances of the type 'Region'.</exception>
    /// <exception cref="MissedConfigurationException">Thrown when an error occurs during the accessing API configuration.</exception>
    /// <exception cref="ResponseDeserializationException">Thrown when an error occurs during the response deserialization.</exception>
    Task<Region> GetRegionAsync(string regionName);

    /// <summary>
    /// Gets all regions of the type "State", but without alert info (<see cref="Region"/> objects are returned with an empty <see cref="IEnumerable{T}"/> of <see cref="Alert"/> 'Alerts').
    /// </summary>
    /// <returns>
    /// An <see cref="IEnumerable{T}"/> of <see cref="Region"/> objects representing all the regions of type "State".
    /// </returns>
    /// <exception cref="HttpRequestException">Thrown when an error occurs during the HTTP request or the response is not in a valid JSON format.</exception>
    /// <exception cref="MissedConfigurationException">Thrown when an error occurs during the accessing API configuration.</exception>
    /// <exception cref="ResponseDeserializationException">Thrown when an error occurs during the response deserialization.</exception>
    Task<IEnumerable<Region>> GetAllRegionsAsync();

    /// <summary>
    /// Gets all regions of the type "State" with active alerts.
    /// </summary>
    /// <returns>
    /// An <see cref="IEnumerable{T}"/> of <see cref="Region"/> objects representing regions with their alert status.
    /// </returns>
    /// <exception cref="HttpRequestException">Thrown when an error occurs during the HTTP request or the response is not in a valid JSON format.</exception>
    /// <exception cref="MissedConfigurationException">Thrown when an error occurs during the accessing API configuration.</exception>
    /// <exception cref="ResponseDeserializationException">Thrown when an error occurs during the response deserialization.</exception>
    Task<IEnumerable<Region>> GetRegionsWithAlertAsync();
}