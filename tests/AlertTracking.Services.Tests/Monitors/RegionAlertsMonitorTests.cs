using AlertTracking.Abstractions.DataAccess.Repositories;
using AlertTracking.Domain.Models;
using AlertTracking.Services.Monitors;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

using Moq;

using NUnit.Framework;

namespace AlertTracking.Services.Tests.Monitors;

[TestFixture]
internal sealed class RegionAlertsMonitorTests
{
    private Mock<IAlertApiRepository> _repositoryMock;
    private RegionAlertMonitor _monitor;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<IAlertApiRepository>();
        _monitor = new RegionAlertMonitor(_repositoryMock.Object);
    }

    [Test]
    [TestCase(true)]
    [TestCase(false)]
    public async Task StartTrackingAsync_AlertStatus_RegionAlertChangedInvoked(bool hasActiveAlerts)
    {
        string regionName = "testName";
        int intervalMilliseconds = 100;
        CancellationTokenSource cancellationTokenSource = new();

        Region region = hasActiveAlerts
            ? CreateRegionWithAlerts(regionName)
            : CreateRegionWithoutAlerts(regionName);

        _repositoryMock.Setup(repo => repo.GetRegionAsync(regionName))
            .ReturnsAsync(region);

        bool regionAlertChangedInvoked = false;
        _monitor.RegionAlertChangedWhileTracking += (sender, args) => regionAlertChangedInvoked = true;

        Task trackingTask = _monitor.StartTrackingAsync(regionName, intervalMilliseconds, cancellationTokenSource.Token);

        await Task.Delay(600);

        cancellationTokenSource.Cancel();

        await trackingTask;

        Assert.That(regionAlertChangedInvoked, Is.EqualTo(hasActiveAlerts));
    }

    [Test]
    public async Task StartTrackingAsync_InvokesGetRegionAlertStatusAsync()
    {
        string regionName = "testName";
        int intervalMilliseconds = 100;
        CancellationTokenSource cancellationTokenSource = new();

        Region region = CreateRegionWithAlerts(regionName);

        _repositoryMock.Setup(repo => repo.GetRegionAsync(regionName))
            .ReturnsAsync(region);

        Task trackingTask = _monitor.StartTrackingAsync(regionName, intervalMilliseconds, cancellationTokenSource.Token);

        await Task.Delay(600);

        cancellationTokenSource.Cancel();

        await trackingTask;

        Mock.Get(_monitor).Verify(m => m.GetRegionAlertStatusAsync(regionName), Times.AtLeastOnce);
    }

    [Test]
    public void StartTrackingAsync_CancellationTokenNull_ThrowsArgumentNullException()
    {
        var regionName = "testRegion";
        var intervalMilliseconds = 100;
        CancellationTokenSource cancellationTokenSource = null!;

        Assert.ThrowsAsync<ArgumentNullException>(async () =>
        {
            await _monitor.StartTrackingAsync(regionName, intervalMilliseconds, cancellationTokenSource.Token);
        });
    }

    [Test]
    [TestCase(0)]
    [TestCase(-10)]
    public void StartTrackingAsync_IntervalMillisecondsOutOfRange_ThrowsArgumentOutOfRangeException(int intervalMilliseconds)
    {
        var regionName = "testRegion";
        CancellationTokenSource cancellationTokenSource = new();

        Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
        {
            await _monitor.StartTrackingAsync(regionName, intervalMilliseconds, cancellationTokenSource.Token);
        });
    }

    [Test]
    public void StartTrackingAsync_RegionNull_ThrowsArgumentNullException()
    {
        Region region = null!;
        int intervalMilliseconds = 100;
        CancellationTokenSource cancellationTokenSource = new();

        Assert.ThrowsAsync<ArgumentNullException>(async () =>
        {
            await _monitor.StartTrackingAsync(region, intervalMilliseconds, cancellationTokenSource.Token);
        });
    }

    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase("   ")]
    public void StartTrackingAsync_RegionNameNullOrEmpty_ThrowsArgumentException(string regionName)
    {
        int intervalMilliseconds = 100;
        CancellationTokenSource cancellationTokenSource = new();

        Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await _monitor.StartTrackingAsync(regionName, intervalMilliseconds, cancellationTokenSource.Token);
        });
    }

    private static Region CreateRegionWithAlerts(string regionName) =>
        new()
        {
            Id = "testId",
            Name = regionName,
            Type = "testType",
            ActiveAlerts = new List<Alert>()
            {
                new Mock<Alert>().Object,
                new Mock<Alert>().Object
            }
        };

    private static Region CreateRegionWithoutAlerts(string regionName) =>
        new()
        {
            Id = "testId",
            Name = regionName,
            Type = "testType",
            ActiveAlerts = new List<Alert>()
        };
}