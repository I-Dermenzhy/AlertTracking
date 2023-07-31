using AlertTracking.Abstractions.Exceptions;
using AlertTracking.Domain.Models;

using System.Text.Json;

namespace AlertTracking.Abstractions.Monitors;

/// <summary>
/// Represents a region alert monitor that tracks and provides information about region alerts.
/// </summary>
/// <remarks>
/// <para>
/// The <see cref="IRegionAlertMonitor"/> interface defines methods and events for monitoring region alerts.
/// Implementing classes are responsible for providing functionality to track and monitor region alerts
/// through events and asynchronous methods.
/// </para>
/// </remarks>
public interface IRegionAlertMonitor : IClonable<IRegionAlertMonitor>
{
    /// <summary>
    /// Gets invoked when the alert status of a region changes while it is being tracked.
    /// </summary>
    event EventHandler<Region>? RegionAlertChangedWhileTracking;

    /// <summary>
    /// Gets invoked when the alert status of a region is checked while it is being tracked, whether the alert status has changed or not.
    /// </summary>
    event EventHandler<Region>? RegionAlertCheckedWhileTracking;

    /// <summary>
    /// Gets all regions along with their alert statuses asynchronously.
    /// </summary>
    /// <returns>
    /// An <see cref="IEnumerable{T}"/> of <see cref="Region"/> representing all regions and their alert statuses.
    /// Each <see cref="Region"/> object contains information about the region and its active alert status.
    /// </returns>
    /// <remarks>
    /// The <see cref="GetAllRegionsAlertStatusAsync"/> method retrieves information about all regions in the monitoring system.
    /// Each <see cref="Region"/> object in the returned collection provides details about the region and whether it has an active alert.
    /// Use this method to get an updated collection of all regions and their current alert statuses.
    /// </remarks>
    /// <exception cref="MissedConfigurationException">
    /// Thrown if the configuration for region alerts is missing or invalid in the configuration sources.
    /// </exception>
    /// <exception cref="HttpRequestException">
    /// Thrown when an error occurs while making an HTTP request to the API for retrieving region alert statuses.
    /// </exception>
    /// <exception cref="JsonException">
    /// Thrown if an error occurs while deserializing the API response to <see cref="Region"/> objects.
    /// </exception>
    Task<IEnumerable<Region>> GetAllRegionsAlertStatusAsync();

    /// <summary>
    /// Gets a collection of regions with active alerts asynchronously.
    /// </summary>
    /// <returns>
    /// An <see cref="IEnumerable{T}"/> of <see cref="Region"/> representing regions with active alerts.
    /// Each <see cref="Region"/> object contains information about the region and its active alert status.
    /// </returns>
    /// <remarks>
    /// The <see cref="GetRegionsWithAlerts"/> method retrieves information about regions with active alerts in the monitoring system.
    /// Each <see cref="Region"/> object in the returned collection provides details about the region and whether it has an active alert.
    /// Use this method to get an updated collection of regions with active alerts.
    /// </remarks>
    /// <exception cref="MissedConfigurationException">
    /// Thrown if the configuration for region alerts is missing or invalid in the configuration sources.
    /// </exception>
    /// <exception cref="HttpRequestException">
    /// Thrown when an error occurs while making an HTTP request to the API for retrieving regions with active alerts.
    /// </exception>
    /// <exception cref="JsonException">
    /// Thrown if an error occurs while deserializing the API response to <see cref="Region"/> objects.
    /// </exception>
    Task<IEnumerable<Region>> GetRegionsWithAlerts();

    /// <summary>
    /// Gets the alert status of a specific region asynchronously based on the region name.
    /// </summary>
    /// <param name="regionName">The name of the region to check the alert status for.</param>
    /// <returns>
    /// A <see cref="bool"/> value indicating whether the region has an active alert (true) or not (false).
    /// </returns>
    /// <remarks>
    /// The <see cref="GetRegionAlertStatusAsync(string)"/> method checks the alert status of a specific region using its name.
    /// It queries the monitoring system to determine whether the region has any active alerts.
    /// </remarks>
    /// <exception cref="ArgumentException">
    /// Thrown if the provided region name is null, empty, or consists only of white space characters.
    /// </exception>
    /// <exception cref="MissedConfigurationException">
    /// Thrown if the configuration for region alerts is missing or invalid in the configuration sources.
    /// </exception>
    /// <exception cref="HttpRequestException">
    /// Thrown when an error occurs while making an HTTP request to the API for retrieving region alert statuses.
    /// </exception>
    /// <exception cref="JsonException">
    /// Thrown if an error occurs while deserializing the API response to <see cref="Region"/> objects.
    /// </exception>
    Task<bool> GetRegionAlertStatusAsync(string regionName);

    /// <summary>
    /// Gets the alert status of a specific region asynchronously.
    /// </summary>
    /// <param name="region">The <see cref="Region"/> object representing the region to check the alert status for.</param>
    /// <returns>
    /// A <see cref="bool"/> value indicating whether the region has an active alert (true) or not (false).
    /// </returns>
    /// <remarks>
    /// The <see cref="GetRegionAlertStatusAsync(Region)"/> method checks the alert status of a specific region using the provided <see cref="Region"/> object.
    /// It queries the monitoring system to determine whether the region has any active alerts.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// Thrown if the provided region object is null.
    /// </exception>
    Task<bool> GetRegionAlertStatusAsync(Region region);

    /// <summary>
    /// Starts asynchronously tracking the alert status of a specific region at a specified interval.
    /// </summary>
    /// <param name="regionName">The name of the region to start tracking.</param>
    /// <param name="intervalMilliseconds">
    /// The interval, in milliseconds, at which the alert status of the region will be checked during tracking.
    /// </param>
    /// <param name="token">
    /// A <see cref="CancellationToken"/> that can be used to stop the tracking operation.
    /// </param>
    /// <remarks>
    /// The <see cref="StartTrackingAsync(string, int, CancellationToken)"/> method initiates asynchronous tracking of the alert status
    /// for the region specified by its name. The alert status of the region will be checked at regular intervals specified by
    /// <paramref name="intervalMilliseconds"/>. If the region's alert status changes, the <see cref="RegionAlertChangedWhileTracking"/>
    /// event will be raised with the updated <see cref="Region"/> object representing the region.
    /// The <see cref="RegionAlertCheckedWhileTracking"/> event is raised whenever the alert status of the region is checked during tracking.
    /// </remarks>
    /// <exception cref="ArgumentException">
    /// Thrown if the provided region name is null, empty, or consists only of white space characters.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if <paramref name="intervalMilliseconds"/> is less than or equal to 0, indicating an invalid interval value.
    /// </exception>
    /// <exception cref="MissedConfigurationException">
    /// Thrown if the configuration for region alerts is missing or invalid in the configuration sources.
    /// </exception>
    /// <exception cref="HttpRequestException">
    /// Thrown when an error occurs while making an HTTP request to the API for retrieving region alert statuses.
    /// </exception>
    /// <exception cref="JsonException">
    /// Thrown if an error occurs while deserializing the API response to <see cref="Region"/> objects.
    /// </exception>
    /// <seealso cref="RegionAlertChangedWhileTracking"/>
    /// <seealso cref="RegionAlertCheckedWhileTracking"/>
    Task StartTrackingAsync(string regionName, int intervalMilliseconds, CancellationToken token);

    /// <summary>
    /// Starts asynchronously tracking the alert status of a specific region at a specified interval.
    /// </summary>
    /// <param name="region">The <see cref="Region"/> object representing the region to start tracking.</param>
    /// <param name="intervalMilliseconds">
    /// The interval, in milliseconds, at which the alert status of the region will be checked during tracking.
    /// </param>
    /// <param name="token">
    /// A <see cref="CancellationToken"/> that can be used to stop the tracking operation.
    /// </param>
    /// <remarks>
    /// The <see cref="StartTrackingAsync(Region, int, CancellationToken)"/> method initiates asynchronous tracking of the alert status
    /// for the region specified by the provided <see cref="Region"/> object. The alert status of the region will be checked at regular
    /// intervals specified by <paramref name="intervalMilliseconds"/>. If the region's alert status changes, the
    /// <see cref="RegionAlertChangedWhileTracking"/> event will be raised with the updated <see cref="Region"/> object representing the region.
    /// The <see cref="RegionAlertCheckedWhileTracking"/> event is raised whenever the alert status of the region is checked during tracking.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// Thrown if the provided region object is null.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if <paramref name="intervalMilliseconds"/> is less than or equal to 0, indicating an invalid interval value.
    /// </exception>
    /// <exception cref="MissedConfigurationException">
    /// Thrown if the configuration for region alerts is missing or invalid in the configuration sources.
    /// </exception>
    /// <exception cref="HttpRequestException">
    /// Thrown when an error occurs while making an HTTP request to the API for retrieving region alert statuses.
    /// </exception>
    /// <exception cref="JsonException">
    /// Thrown if an error occurs while deserializing the API response to <see cref="Region"/> objects.
    /// </exception>
    /// <seealso cref="RegionAlertChangedWhileTracking"/>
    /// <seealso cref="RegionAlertCheckedWhileTracking"/>
    Task StartTrackingAsync(Region region, int intervalMilliseconds, CancellationToken token);
}