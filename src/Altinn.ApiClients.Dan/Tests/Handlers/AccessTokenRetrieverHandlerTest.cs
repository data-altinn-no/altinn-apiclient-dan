using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Altinn.ApiClients.Dan.Handlers;
using Altinn.ApiClients.Dan.Interfaces;
using FakeItEasy;
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

            _mockAccessTokenRetriever = A.Fake<IAccessTokenRetriever>();

            A.CallTo(() => _mockAccessTokenRetriever.GetAccessToken(A<bool>.Ignored))
                .Returns(Task.FromResult("nunit-token"));
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
            A.CallTo(() => _mockAccessTokenRetriever.GetAccessToken(false))
                .MustHaveHappenedOnceExactly();        
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
            A.CallTo(() => _mockAccessTokenRetriever.GetAccessToken(true))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() => _mockAccessTokenRetriever.GetAccessToken(false))
                .MustHaveHappenedOnceExactly();
        }

        private static DelegatingHandler GetInnerHandlerMock(HttpRequestMessage request, HttpStatusCode returnsStatusCode)
        {
            var innerHandlerFake = A.Fake<DelegatingHandler>();

            A.CallTo(innerHandlerFake)
                .WithReturnType<Task<HttpResponseMessage>>()
                .Where(call =>
                    call.Method.Name == "SendAsync" &&
                    call.Arguments[0].Equals(request))
                .Returns(Task.FromResult(new HttpResponseMessage(returnsStatusCode)));

            return innerHandlerFake;

        }
    }
}