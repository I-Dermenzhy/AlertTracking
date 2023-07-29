using AlertTracking.Abstractions.Configuration;
using AlertTracking.Services.DataAccess.HttpRequests;

using Moq;

using NUnit.Framework;

namespace AlertTracking.Services.Tests.DataAccess.HttpRequests;

[TestFixture]
internal sealed class ApiRequestProviderTests
{
    private ApiRequestProvider _requestProvider;

    [SetUp]
    public void SetUp()
    {
        Mock<IApiConfigurationProvider> configurationProvider = new();
        _requestProvider = new(configurationProvider.Object);
    }

    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase("  ")]
    public void GetRegionAlertsRequest_NullOrEmptyRegionName_ThrowsArgumentException(string regionName) =>
        Assert.That(() => _requestProvider.GetRegionAlertsRequest(regionName), Throws.InstanceOf<ArgumentException>());
}
