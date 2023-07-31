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
    /// <returns>The task result which contains the last action index.</returns>
    Task<long> GetLastActionIndexAsync();

    /// <summary>
    /// Gets a region with its alert status.
    /// </summary>
    /// <param name="regionName">The name of the region to retrieve.</param>
    /// <returns>
    /// The task result which contains a <see cref="Region"/> object representing the region with its alert status.
    /// </returns>
    Task<Region> GetRegionAsync(string regionName);

    /// <summary>
    /// Gets all regions of the type "State", but without alert info (<see cref="Region"/> are returned with an empty <see cref="IEnumerable{T}"/> of <see cref="Alert"/> 'Alerts').
    /// </summary>
    /// <returns>
    /// The task result which contains an <see cref="IEnumerable{T}"/> of <see cref="Region"/> objects representing all the regions of type "State".
    /// </returns>
    Task<IEnumerable<Region>> GetAllRegionsAsync();

    /// <summary>
    /// Gets all regions of the type "State" with active alerts.
    /// </summary>
    /// <returns>
    /// The task result which contains an <see cref="IEnumerable{T}"/> of <see cref="Region"/> objects representing regions with their alert status.
    /// </returns>
    Task<IEnumerable<Region>> GetRegionsWithAlertAsync();
}