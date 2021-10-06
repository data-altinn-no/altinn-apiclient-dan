using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Altinn.ApiClients.Dan.Interfaces;

namespace Altinn.ApiClients.Dan.Handlers
{
    public class AccessTokenRetrieverHandler : DelegatingHandler
    {
        private readonly IAccessTokenRetriever _accessTokenRetriever;

        public AccessTokenRetrieverHandler(IAccessTokenRetriever accessTokenRetriever)
        {
            _accessTokenRetriever = accessTokenRetriever;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Headers.Authorization == null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _accessTokenRetriever.GetAccessToken());
            }

            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode != HttpStatusCode.Unauthorized) return response;
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _accessTokenRetriever.GetAccessToken(true));
                response = await base.SendAsync(request, cancellationToken);
            }

            return response;
        }
    }
}
