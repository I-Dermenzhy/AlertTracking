using AlertTracking.Domain.Models;

namespace AlertTracking.Abstractions.Monitors;

public interface IRegionAlertMonitor : IClonable<IRegionAlertMonitor>
{
    event EventHandler<Region>? RegionAlertChangedWhileTracking;
    event EventHandler<Region>? RegionAlertCheckedWhileTracking;

    Task<IEnumerable<Region>> GetAllRegionsAlertStatusAsync();

    Task<IEnumerable<Region>> GetRegionsWithAlerts();

    Task<bool> GetRegionAlertStatusAsync(string regionName);
    Task<bool> GetRegionAlertStatusAsync(Region region);

    Task StartTrackingAsync(string regionName, int intervalMilliseconds, CancellationToken token);
    Task StartTrackingAsync(Region region, int intervalMilliseconds, CancellationToken token);

    //Task StartTrackingAsync(int intervalMilliseconds, CancellationToken token);
}