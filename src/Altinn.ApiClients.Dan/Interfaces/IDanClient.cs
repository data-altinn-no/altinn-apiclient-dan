using System.Collections.Generic;
using System.Threading.Tasks;
using Altinn.ApiClients.Dan.Models;

namespace Altinn.ApiClients.Dan.Interfaces
{
    public interface IDanClient
    {
        IDanConfiguration Configuration { get; set; }

        Task<DataSet> GetDataSet(string dataSetName, string subject,
            string requestor = null, Dictionary<string, string> parameters = null);

        Task<T> GetDataSet<T>(string dataSetName, string subject,
            string requestor = null, Dictionary<string, string> parameters = null, string deserializeField = null) where T : new();

        Task<Accreditation> CreateDataSetRequest(List<DataSetRequest> dataSetRequest, string subject,
            string requestor = null, string consentRedirectUrl = null);

        Task<DataSet> GetDataSetFromAccreditation(string accreditationguid, string datasetname);

        Task<T> GetDataSetFromAccreditation<T>(string accreditationguid, string datasetname, string deserializeField = null) where T : new();

        Task<List<DataSetRequestStatus>> GetRequestStatus(string accreditationGuid, string dataSetName);
        
        Task<List<DataSetRequestStatus>> GetRequestStatus(string accreditationGuid);
    }
}