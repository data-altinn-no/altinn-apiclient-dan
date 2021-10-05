using System.Collections.Generic;
using System.Threading.Tasks;
using Altinn.ApiClients.Dan.Models;

namespace Altinn.ApiClients.Dan.Interfaces
{
    public interface IDanClient
    {
        Task<DataSet> GetSynchronousDataset(string dataSetName, string subject,
            string requestor = null, Dictionary<string, string> parameters = null);

        Task<Accreditation> CreateAsynchronousDatasetRequest(DataSetRequest dataSetRequest, string subject,
            string requestor = null);

        Task<DataSet> GetAsynchronousDataset(string accreditationguid, string datasetname);

        Task<List<DataSetRequestStatus>> GetRequestStatus(string accreditationGuid, string dataSetName);
        
        Task<List<DataSetRequestStatus>> GetRequestStatus(string accreditationGuid);
    }
}