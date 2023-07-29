using AlertTracking.Domain.Dtos;
using AlertTracking.Domain.Models;

namespace AlertTracking.Abstractions.Monitors;

public interface IRegionAlertMonitor : IClonable<IRegionAlertMonitor>
{
    event EventHandler<RegionAlertArgs>? RegionAlertChangedWhileTracking;
    event EventHandler<RegionAlertArgs>? RegionAlertCheckedWhileTracking;

    Task<IEnumerable<RegionAlertArgs>> GetAllRegionsAlertStatusAsync();

    Task<IEnumerable<RegionAlertArgs>> GetRegionsWithAlerts();

    Task<bool> GetRegionAlertStatusAsync(string regionName);
    Task<bool> GetRegionAlertStatusAsync(Region region);

    Task StartTrackingAsync(string regionName, int intervalMilliseconds, CancellationToken token);
    Task StartTrackingAsync(Region region, int intervalMilliseconds, CancellationToken token);

    //Task StartTrackingAsync(int intervalMilliseconds, CancellationToken token);
}