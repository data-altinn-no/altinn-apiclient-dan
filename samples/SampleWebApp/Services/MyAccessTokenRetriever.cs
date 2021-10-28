using System.Threading.Tasks;
using Altinn.ApiClients.Dan.Interfaces;

namespace SampleWebApp.Services
{
    public class MyAccessTokenRetriever : IAccessTokenRetriever
    {
        public Task<string> GetAccessToken(bool forceRefresh = false)
        {
            return Task.FromResult("my-fancy-token");
        }
    }
}