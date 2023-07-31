using AlertTracking.Domain.Models;

using Moq;

using NUnit.Framework;

namespace AlertTracking.Domain.Tests.Models;

[TestFixture]
internal sealed class RegionTests
{
    [Test]
    public void IsAlert_EmptyActiveAlertsCollection_ReturnsFalse()
    {
        Region region = new()
        {
            Id = "Test",
            Name = "Test",
            Type = "Test",
            ActiveAlerts = Enumerable.Empty<Alert>()
        };

        Assert.That(region.IsAlert, Is.False);
    }

    [Test]
    public void IsAlert_FilledActiveAlertsCollection_ReturnsTrue()
    {
        Region region = new()
        {
            Id = "Test",
            Name = "Test",
            Type = "Test",
            ActiveAlerts = new List<Alert>()
            {
                new Mock<Alert>().Object
            }
        };

        Assert.That(region.IsAlert, Is.True);
    }
}
