using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Altinn.ApiClients.Dan.Interfaces;
using Altinn.ApiClients.Dan.Models;
using Altinn.ApiClients.Dan.Models.Enums;
using Refit;

namespace Altinn.ApiClients.Dan.Services
{
    public class DanClient : IDanClient
    {
        private readonly IDanApi _danApi;

        public IDanConfiguration Configuration { get; set; }

        public DanClient(IDanApi danApi, IDanConfiguration danConfiguration = null)
        {
            Configuration = danConfiguration ?? new DefaultDanConfiguration();
            _danApi = danApi;
        }

        public async Task<DataSet> GetDataSet(string dataSetName, string subject,
            string requestor = null, Dictionary<string, string> parameters = null)
        {
            try
            {
                return await _danApi.GetDirectharvest(dataSetName, subject, requestor, parameters);
            }
            catch (ApiException ex)
            {
                throw DanException.FromApiException(ex); 
            }
        }

        public async Task<T> GetDataSet<T>(string dataSetName, string subject,
            string requestor = null, Dictionary<string, string> parameters = null, string deserializeField = null) where T : new()
        {
            try
            {
                var result = await GetDataSet(dataSetName, subject, requestor, parameters);
                return GetDataSetResultAsTyped<T>(result, deserializeField);
            }
            catch (ApiException ex)
            {
                throw DanException.FromApiException(ex);
            }
        }

        public async Task<Accreditation> CreateDataSetRequest(List<DataSetRequest> dataSetRequests, string subject,
            string requestor = null, string consentRedirectUrl = null)
        {
            var authorizationRequest = new AuthorizationRequest()
            {
                DataSetRequests = dataSetRequests,
                Subject = subject,
                Requestor = requestor,
                ConsentReceiptRedirectUrl = consentRedirectUrl                
            };

            try
            {
                return await _danApi.PostAuthorization(authorizationRequest);
            }
            catch (ApiException ex)
            {
                throw DanException.FromApiException(ex);
            }
        }
        public async Task<DataSet> GetDataSetFromAccreditation(string accreditationguid, string datasetname)
        {
            try
            {
                return await _danApi.GetEvidence(accreditationguid, datasetname);
            }
            catch (ApiException ex)
            {
                throw DanException.FromApiException(ex);
            }
        }

        public async Task<T> GetDataSetFromAccreditation<T>(string accreditationguid, string datasetname, string deserializeField = null) where T : new()
        {
            try
            {
                var result = await GetDataSetFromAccreditation(accreditationguid, datasetname);
                return GetDataSetResultAsTyped<T>(result, deserializeField);
            }
            catch (ApiException ex)
            {
                throw DanException.FromApiException(ex);
            }
        }

        public async Task<List<DataSetRequestStatus>> GetRequestStatus(string accreditationGuid, string dataSetName)
        {
            try
            {
                var dataSetRequestStatuses = await _danApi.GetEvidenceStatus(accreditationGuid);
                return dataSetRequestStatuses.Where(status => status.DataSetName.Equals(dataSetName)).ToList();
            }
            catch (ApiException ex)
            {
                throw DanException.FromApiException(ex);
            }
        }

        public async Task<List<DataSetRequestStatus>> GetRequestStatus(string accreditationGuid)
        {
            try
            {
                return await _danApi.GetEvidenceStatus(accreditationGuid);
            }
            catch (ApiException ex)
            {
                throw DanException.FromApiException(ex);
            }
        }

        private T GetDataSetResultAsTyped<T>(DataSet result, string deserializeField) where T : new()
        {
            try
            {
                // If deserializeField is supplied, attempt to deserialize that and ignore everything else
                if (deserializeField != null)
                {
                    var deserializeDataSetValue =
                        result.Values.FirstOrDefault(x => x.Name == deserializeField);
                    if (deserializeDataSetValue == null)
                    {
                        throw new DanException(
                            $"Target field for deserialization '{deserializeField}' was not present in the dataset");
                    }

                    return deserializeDataSetValue.Value == null
                        ? default
                        : Configuration.Deserializer.Deserialize<T>(deserializeDataSetValue.Value.ToString()!);
                }

                // If deserializeField is not supplied, and the first field in the return is of type "JsonSchema", attempt to deserialize that and ignore everything else
                var firstDataSetValue = result.Values.First();
                if (firstDataSetValue.ValueType == DataSetValueType.JsonSchema)
                {
                    return firstDataSetValue.Value == null
                        ? default
                        : Configuration.Deserializer.Deserialize<T>(firstDataSetValue.Value.ToString()!);
                }

                // TODO!
                // Attempt to manually map the dataset to the supplied type via reflection. This will require mapping of
                // all DateSetValueTypes to appropiate .NET types.

                throw new DanException(
                    $"Mapping to model '{typeof(T).Name}' requires the dataset to have a field of type 'JsonSchema', which is either the first field or specified via the 'deserializeField' parameter");
            }
            catch (Exception ex)
            {
                throw new DanException("Unable to deserialize to requested type: " + ex.Message, ex);
            }
        }
    }
}