namespace AlertTracking.Abstractions.Monitors;

public interface IStatusUpdatesMonitor
{
    /// <summary>
    /// Gets invoked when a status update is detected (last action index changed).
    /// </summary>
    event EventHandler? StatusUpdated;

    /// <summary>
    /// Checks for status updates asynchronously.
    /// </summary>
    /// <remarks>
    /// This method is used to check for status updates from an external source asynchronously.
    /// Implementations of this method should retrieve the latest status information and raise the
    /// <see cref="StatusUpdated"/> event if a new status update is detected.
    /// <para>
    /// Note that this method is used to perform one check per call, not the constant tracking.
    /// </para>
    /// </remarks>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task CheckStatusUpdatedAsync();

    /// <summary>
    /// Starts tracking status updates asynchronously.
    /// </summary>
    /// <remarks>
    /// This method starts the tracking process for status updates from an external source. The tracking
    /// is performed at the specified interval in milliseconds, and the operation can be canceled using
    /// the provided <see cref="CancellationToken"/>.
    /// </remarks>
    /// <param name="intervalMilliseconds">The interval in milliseconds between status update checks.</param>
    /// <param name="token">The <see cref="CancellationToken"/> to use for canceling the tracking operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the <paramref name="intervalMilliseconds"/> is less than or equal to 0.</exception>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="token"/> is null.</exception>
    Task StartTrackingAsync(int intervalMilliseconds, CancellationToken token);
}