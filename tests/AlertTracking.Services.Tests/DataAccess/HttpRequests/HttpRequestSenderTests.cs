using AlertTracking.Services.DataAccess.HttpRequests;

using Moq;

using NUnit.Framework;

namespace AlertTracking.Services.Tests.DataAccess.HttpRequests;

[TestFixture]
internal sealed class HttpRequestSenderTests
{
    private HttpRequestSender _sender;

    [SetUp]
    public void SetUp()
    {
        Mock<IHttpClientFactory> clientFactoryMock = new();
        _sender = new(clientFactoryMock.Object);
    }

    [Test]
    public void SendHttpRequestAsync_WithNullRequest_ThrowsArgumentNullException() =>
        Assert.That(async () => await _sender.SendHttpRequestAsync(null!, "testToken"), Throws.ArgumentNullException);

    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase("  ")]
    public void SendHttpRequestAsync_NullOrEmptyToken_ThrowsArgumentException(string token) =>
        Assert.That(async () => await _sender.SendHttpRequestAsync(null!, token), Throws.InstanceOf<ArgumentException>());
}
