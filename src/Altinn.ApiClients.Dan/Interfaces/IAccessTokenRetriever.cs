using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altinn.ApiClients.Dan.Interfaces
{
    public interface IAccessTokenRetriever
    {
        Task<string> GetAccessToken(bool forceRefresh = false);
    }
}
