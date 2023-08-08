using AlertTracking.Abstractions.Monitors;
using AlertTracking.Domain.Models;

using Microsoft.AspNetCore.SignalR;

namespace AlertTracking.WebMapDemoUI.Hubs;

internal sealed class AlertHub : Hub
{
    private readonly IRegionAlertMonitor _alertMonitor;
    private readonly ILogger<AlertHub> _logger;

    public AlertHub(IRegionAlertMonitor alertMonitor, ILogger<AlertHub> logger)
    {
        _alertMonitor = alertMonitor ?? throw new ArgumentNullException(nameof(alertMonitor));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task SendRegions()
    {
        try
        {
            IEnumerable<Region> regions = await _alertMonitor.GetAllRegionsAlertStatusAsync();

            await Clients.All.SendAsync("ReceiveRegions", regions);
        }
        catch (SystemException ex)
        {
            _logger.LogCritical("{exception message}\n{exception stack trace}", ex.Message, ex.StackTrace);
        }
        catch (Exception ex)
        {
            _logger.LogError("{exception message}\n{exception stack trace}", ex.Message, ex.StackTrace);
        }
    }
}
