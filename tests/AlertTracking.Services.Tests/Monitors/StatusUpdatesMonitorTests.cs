using AlertTracking.Abstractions.DataAccess.Repositories;
using AlertTracking.Services.Monitors;

using Moq;

using NUnit.Framework;

namespace AlertTracking.UnitTests.Monitors;

[TestFixture]
internal sealed class StatusUpdatesMonitorTests
{
    private Mock<IAlertApiRepository> _repositoryMock;
    private StatusUpdatesMonitor _monitor;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<IAlertApiRepository>();
        _monitor = new StatusUpdatesMonitor(_repositoryMock.Object);
    }

    [Test]
    public async Task TrackStatusUpdatesAsync_LastActionIndexChanged_StatusUpdatedInvoked()
    {
        long initialActionIndex = 0;
        long updatedActionIndex = 20;

        int intervalMilliseconds = 100;
        CancellationTokenSource cancellationTokenSource = new();

        _repositoryMock.Setup(r => r.GetLastActionIndexAsync())
            .ReturnsAsync(initialActionIndex);

        bool statusUpdatedInvoked = false;
        _monitor.StatusUpdated += (sender, args) => statusUpdatedInvoked = true;

        Task trackingTask = _monitor.StartTrackingAsync(intervalMilliseconds, cancellationTokenSource.Token);

        await Task.Delay(400);

        _repositoryMock.Setup(r => r.GetLastActionIndexAsync())
            .ReturnsAsync(updatedActionIndex);

        await Task.Delay(400);

        cancellationTokenSource.Cancel();
        await trackingTask;

        Assert.That(statusUpdatedInvoked, Is.True);
    }

    [Test]
    public async Task TrackStatusUpdatesAsync_LastActionIndexNotChanged_StatusUpdatedNotInvoked()
    {
        long actionIndex = 0;

        int intervalMilliseconds = 100;
        CancellationTokenSource cancellationTokenSource = new();

        _repositoryMock.Setup(r => r.GetLastActionIndexAsync())
            .ReturnsAsync(actionIndex);

        bool statusUpdatedInvoked = false;
        _monitor.StatusUpdated += (sender, args) => statusUpdatedInvoked = true;

        Task trackingTask = _monitor.StartTrackingAsync(intervalMilliseconds, cancellationTokenSource.Token);

        await Task.Delay(400);

        _repositoryMock.Setup(r => r.GetLastActionIndexAsync())
            .ReturnsAsync(actionIndex);

        await Task.Delay(400);

        cancellationTokenSource.Cancel();
        await trackingTask;

        Assert.That(statusUpdatedInvoked, Is.False);
    }
}
