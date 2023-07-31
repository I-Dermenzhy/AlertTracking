using AlertTracking.Abstractions.Configuration;
using AlertTracking.Services.Configuration;

using Microsoft.Extensions.Configuration;

using NUnit.Framework;

namespace AlertTracking.Services.Tests.Configuration;

[TestFixture]
internal sealed class ApiConfigurationProviderTests
{
    private IApiConfigurationProvider _provider;

    [SetUp]
    public void SetUp()
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new[]
            {
                new KeyValuePair<string, string>("AUTHORIZATION_TOKEN", "testToken"),
                new KeyValuePair<string, string>("UkraineAlertApi:Url", "testUrl"),
                new KeyValuePair<string, string>("UkraineAlertApi:Endpoints:RegionsWithAlerts", "testRegionsWithAlerts"),
                new KeyValuePair<string, string>("UkraineAlertApi:Endpoints:Regions", "testRegions"),
                new KeyValuePair<string, string>("UkraineAlertApi:Endpoints:Status", "testStatus"),
                new KeyValuePair<string, string>("UkraineAlertApi:RegionIds:Region1", "5"),
                new KeyValuePair<string, string>("UkraineAlertApi:RegionIds:Region2", "10"),
            }!)
            .Build();

        _provider = new ApiConfigurationProvider(configuration);
    }

    [Test]
    public void GetApiAuthorizationToken_WithValidConfiguration_ReturnsToken()
    {
        string expectedResult = "testToken";

        string token = _provider.GetApiAuthorizationToken();

        Assert.That(token, Is.EqualTo(expectedResult));
    }

    [Test]
    public void GetApiAuthorizationToken_WithMissingConfiguration_ThrowsArgumentException()
    {
        var provider = GetProviderWithMissingConfiguration();

        Assert.That(provider.GetApiAuthorizationToken, Throws.Exception);
    }

    [Test]
    public void GetApiEndpoints_WithValidConfiguration_ReturnsEndpoints()
    {
        Endpoints endpoints = _provider.GetApiEndpoints();

        Assert.That(endpoints, Is.Not.Null);
        Assert.That(endpoints.RegionsWithAlerts, Is.EqualTo("testRegionsWithAlerts"));
    }

    [Test]
    public void GetApiEndpoints_WithMissingConfiguration_ThrowsArgumentException()
    {
        var provider = GetProviderWithMissingConfiguration();

        Assert.That(provider.GetApiEndpoints, Throws.Exception);
    }

    [Test]
    public void GetApiUrl_WithValidConfiguration_ReturnsUrl()
    {
        string expectedResult = "testUrl";

        string url = _provider.GetApiUrl();

        Assert.That(url, Is.EqualTo(expectedResult));
    }

    [Test]
    public void GetApiUrl_WithMissingConfiguration_ThrowsArgumentException()
    {
        var provider = GetProviderWithMissingConfiguration();

        Assert.That(provider.GetApiUrl, Throws.Exception);
    }

    [Test]
    public void GetRegionNameIdPairs_WithValidConfiguration_ReturnsRegionRegionNameIdPairs()
    {
        Dictionary<string, string> expectedResult = new()
        {
            { "Region1", "5" } ,
            { "Region2", "10" }
        };

        Dictionary<string, string> regionNameIdPairs = _provider.GetRegionNameIdPairs();

        Assert.That(regionNameIdPairs, Is.EquivalentTo(expectedResult));
    }

    [Test]
    public void GetRegionNameIdPairs_WithMissingConfiguration_ThrowsArgumentException()
    {
        var provider = GetProviderWithMissingConfiguration();

        Assert.That(provider.GetRegionNameIdPairs, Throws.Exception);
    }

    [Test]
    [TestCase("Region1", "5")]
    [TestCase("Region2", "10")]
    public void GetRegionId_WithValidConfiguration_ReturnsId(string regionName, string expectedResult)
    {
        string id = _provider.GetRegionId(regionName);

        Assert.That(id, Is.EqualTo(expectedResult));
    }

    [Test]
    public void GetRegionId_WithMissingConfiguration_ThrowsArgumentException()
    {
        var provider = GetProviderWithMissingConfiguration();

        Assert.That(() => provider.GetRegionId("Test"), Throws.Exception);
    }

    [Test]
    public void GetRegionNames_WithValidConfiguration_ReturnsRegionNames()
    {
        IEnumerable<string> expectedResult = new List<string>() { "Region1", "Region2" };

        IEnumerable<string> regionNames = _provider.GetRegionNames();

        Assert.That(regionNames, Is.EqualTo(expectedResult));
    }

    [Test]
    public void GetRegionNames_WithMissingConfiguration_ThrowsArgumentException()
    {
        var provider = GetProviderWithMissingConfiguration();

        Assert.That(provider.GetRegionNames, Throws.Exception);
    }

    private static ApiConfigurationProvider GetProviderWithMissingConfiguration()
    {
        IConfiguration configuration = new ConfigurationBuilder().Build();

        return new(configuration);
    }
}