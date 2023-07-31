using AlertTracking.Abstractions.DataAccess.Repositories;
using AlertTracking.Abstractions.Monitors;

namespace AlertTracking.Services.Monitors;

public class StatusUpdatesMonitor : IStatusUpdatesMonitor
{
    private readonly IAlertApiRepository _repository;

    private long _lastActionIndex;

    public StatusUpdatesMonitor(IAlertApiRepository repository) =>
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    public event EventHandler? StatusUpdated;

    public async Task CheckStatusUpdatedAsync()
    {
        long lastActionIndex = await _repository.GetLastActionIndexAsync();

        if (IsLactActionIndexChanged(lastActionIndex))
            OnLastActionIndexChanged(lastActionIndex);
    }

    public async Task StartTrackingAsync(int intervalMilliseconds, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(token, nameof(token));

        if (intervalMilliseconds <= 0)
            throw new ArgumentOutOfRangeException(nameof(intervalMilliseconds));

        try
        {
            while (!token.IsCancellationRequested)
            {
                await CheckStatusUpdatedAsync();

                await Task.Delay(intervalMilliseconds, token);
            }
        }
        catch (OperationCanceledException)
        {
            // Tracking was cancelled, ignore the exception
        }
    }

    private void OnLastActionIndexChanged(long lastActionIndex)
    {
        StatusUpdated?.Invoke(this, EventArgs.Empty);
        _lastActionIndex = lastActionIndex;
    }

    private bool IsLactActionIndexChanged(long lastActionIndex) => lastActionIndex != _lastActionIndex;
}