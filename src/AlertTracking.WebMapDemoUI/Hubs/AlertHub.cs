using AlertTracking.Abstractions.Monitors;
using AlertTracking.Domain.Models;

using Microsoft.AspNetCore.SignalR;

namespace AlertTracking.WebMapDemoUI.Hubs;

internal sealed class AlertHub : Hub
{
    private readonly IRegionAlertMonitor _alertMonitor;

    public AlertHub(IRegionAlertMonitor alertMonitor) =>
        _alertMonitor = alertMonitor ?? throw new ArgumentNullException(nameof(alertMonitor));

    public async Task SendRegions()
    {
        IEnumerable<Region> regions = await _alertMonitor.GetAllRegionsAlertStatusAsync();

        await Clients.All.SendAsync("ReceiveRegions", regions);
    }
}
