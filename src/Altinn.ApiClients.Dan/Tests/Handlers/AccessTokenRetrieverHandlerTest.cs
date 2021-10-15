using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Altinn.ApiClients.Dan.Handlers;
using Altinn.ApiClients.Dan.Interfaces;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace Tests.Handlers
{
    [TestFixture]
    public class AccessTokenRetrieverHandlerTest
    {
        private HttpRequestMessage _request;
        private IAccessTokenRetriever _mockAccessTokenRetriever;

        [SetUp]
        public void Setup()
        {
            _request = new HttpRequestMessage();
            _mockAccessTokenRetriever = Mock.Of<IAccessTokenRetriever>();
            Mock.Get(_mockAccessTokenRetriever).Setup(r => r.GetAccessToken(It.IsAny<bool>())).Returns(Task.FromResult("nunit-token"));
        }

        [Test]
        public async Task SendAsync_ok()
        {
            var handler = new AccessTokenRetrieverHandler(_mockAccessTokenRetriever)
            {
                InnerHandler = GetInnerHandlerMock(_request, HttpStatusCode.OK)
            };
            var invoker = new HttpMessageInvoker(handler);

            var response = await invoker.SendAsync(_request, default);
            
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(_request.Headers.Authorization?.Parameter, Does.Contain("nunit-token"));
            Assert.That(_request.Headers.Authorization?.Scheme, Does.Contain("Bearer"));
            Mock.Get(_mockAccessTokenRetriever).Verify(x => x.GetAccessToken(false), Times.Once);
        }

        [Test]
        public async Task SendAsync_unauthorized_ForceRefreshToken()
        {
            var handler = new AccessTokenRetrieverHandler(_mockAccessTokenRetriever)
            {
                InnerHandler = GetInnerHandlerMock(_request, HttpStatusCode.Unauthorized)
            };
            var invoker = new HttpMessageInvoker(handler);

            var response = await invoker.SendAsync(_request, default);
            
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
            Assert.That(_request.Headers.Authorization?.Parameter, Does.Contain("nunit-token"));
            Assert.That(_request.Headers.Authorization?.Scheme, Does.Contain("Bearer"));
            Mock.Get(_mockAccessTokenRetriever).Verify(x => x.GetAccessToken(true), Times.Once);
            Mock.Get(_mockAccessTokenRetriever).Verify(x => x.GetAccessToken(false), Times.Once);
        }

        private static DelegatingHandler GetInnerHandlerMock(HttpRequestMessage request, HttpStatusCode returnsStatusCode)
        {
            var innerHandlerMock = Mock.Of<DelegatingHandler>(MockBehavior.Strict);
            Mock.Get(innerHandlerMock)
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", request, ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(returnsStatusCode));
            return innerHandlerMock;
        }
    }
}