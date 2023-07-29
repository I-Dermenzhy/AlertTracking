using AlertTracking.Abstractions.Monitors;
using AlertTracking.Domain.Dtos;

using Microsoft.AspNetCore.SignalR;

namespace AlertTracking.WebMapDemoUI.Hubs;

internal sealed class AlertHub : Hub
{
    private readonly IRegionAlertMonitor _alertMonitor;

    public AlertHub(IRegionAlertMonitor alertMonitor) =>
        _alertMonitor = alertMonitor ?? throw new ArgumentNullException(nameof(alertMonitor));

    public async Task SendRegions()
    {
        IEnumerable<RegionAlertArgs> regions = await _alertMonitor.GetAllRegionsAlertStatusAsync();

        await Clients.All.SendAsync("ReceiveRegions", regions);
    }
}
