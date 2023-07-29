namespace AlertTracking.Abstractions.Monitors;

public interface IStatusUpdatesMonitor
{
    event EventHandler? StatusUpdated;

    Task CheckStatusUpdatedAsync();
    Task StartTrackingAsync(int intervalMilliseconds, CancellationToken token);
}