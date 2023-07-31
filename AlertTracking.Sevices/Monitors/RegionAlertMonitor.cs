using AlertTracking.Abstractions.DataAccess.Repositories;
using AlertTracking.Abstractions.Monitors;
using AlertTracking.Domain.Models;

namespace AlertTracking.Services.Monitors;

public class RegionAlertMonitor : IRegionAlertMonitor
{
    private readonly IAlertApiRepository _repository;

    private bool _isAlert;

    public RegionAlertMonitor(IAlertApiRepository repository) =>
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    public event EventHandler<Region>? RegionAlertChangedWhileTracking;
    public event EventHandler<Region>? RegionAlertCheckedWhileTracking;

    public IRegionAlertMonitor Clone() => new RegionAlertMonitor(_repository);

    public async Task<IEnumerable<Region>> GetAllRegionsAlertStatusAsync()
    {
        IEnumerable<Region> allRegions = await _repository.GetAllRegionsAsync();
        IEnumerable<Region> regionsWithAlerts = await _repository.GetRegionsWithAlertAsync();

        return allRegions.Select(region =>
            regionsWithAlerts.FirstOrDefault(r => r.Name == region.Name) ?? region);
    }

    public async Task<IEnumerable<Region>> GetRegionsWithAlerts() => await _repository.GetRegionsWithAlertAsync();

    public async Task<bool> GetRegionAlertStatusAsync(string regionName)
    {
        if (string.IsNullOrWhiteSpace(regionName))
            throw new ArgumentException($"'{nameof(regionName)}' cannot be null or whitespace.", nameof(regionName));

        Region region = await GetRegionByNameAsync(regionName);

        return IsAlert(region);
    }

    public async Task<bool> GetRegionAlertStatusAsync(Region region)
    {
        ArgumentNullException.ThrowIfNull(region, nameof(region));

        return await GetRegionAlertStatusAsync(region.Name);
    }

    public async Task StartTrackingAsync(string regionName, int intervalMilliseconds, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(regionName))
            throw new ArgumentException($"'{nameof(regionName)}' cannot be null or whitespace.", nameof(regionName));

        if (intervalMilliseconds <= 0)
            throw new ArgumentOutOfRangeException(nameof(intervalMilliseconds));

        try
        {
            while (!token.IsCancellationRequested)
            {
                Region region = await GetRegionByNameAsync(regionName);

                RegionAlertCheckedWhileTracking?.Invoke(this, region);

                bool isAlert = IsAlert(region);

                if (isAlert != _isAlert)
                    OnRegionAlertStatusChanged(region);

                await Task.Delay(intervalMilliseconds, token);
            }
        }
        catch (OperationCanceledException)
        {
            // Tracking was cancelled, ignore the exception
        }
    }

    public async Task StartTrackingAsync(Region region, int intervalMilliseconds, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(region, nameof(region));

        await StartTrackingAsync(region.Name, intervalMilliseconds, cancellationToken);
    }

    private static bool IsAlert(Region region) => region.ActiveAlerts.Any();

    private async Task<Region> GetRegionByNameAsync(string regionName) => await _repository.GetRegionAsync(regionName);

    private void OnRegionAlertStatusChanged(Region region)
    {
        _isAlert = !_isAlert;

        RegionAlertChangedWhileTracking?.Invoke(this, region);
    }
}
