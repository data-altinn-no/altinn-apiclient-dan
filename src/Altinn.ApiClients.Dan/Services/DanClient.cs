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
    /// <inheritdoc />
    public class DanClient : IDanClient
    {
        private readonly IDanApi _danApi;

        /// <inheritdoc />
        public IDanConfiguration Configuration { get; set; }

        public DanClient(IDanApi danApi, IDanConfiguration danConfiguration = null)
        {
            Configuration = danConfiguration ?? new DefaultDanConfiguration();
            _danApi = danApi;
        }

        /// <inheritdoc />
        public async Task<DataSet> GetDataSet(
            string dataSetName,
            string subject,
            string requestor = null,
            Dictionary<string, string> parameters = null,
            TokenOnBehalfOf? tokenOnBehalfOf = null,
            bool reuseToken = false,
            string forwardAccessToken = null)
        {
            try
            {
                return await _danApi.GetDirectharvest(
                    dataSetName,
                    subject,
                    requestor,
                    parameters,
                    tokenOnBehalfOf,
                    reuseToken,
                    forwardAccessToken);
            }
            catch (ApiException ex)
            {
                throw DanException.FromApiException(ex); 
            }
        }

        /// <inheritdoc />
        public async Task<T> GetDataSet<T>(
            string dataSetName,
            string subject,
            string requestor = null,
            Dictionary<string, string> parameters = null,
            string deserializeField = null,
            TokenOnBehalfOf? tokenOnBehalfOf = null,
            bool reuseToken = false,
            string forwardAccessToken = null) where T : new()
        {
            try
            {
                var result = await GetDataSet(
                    dataSetName,
                    subject,
                    requestor,
                    parameters,
                    tokenOnBehalfOf,
                    reuseToken,
                    forwardAccessToken);
                return GetDataSetAsTyped<T>(result, deserializeField);
            }
            catch (ApiException ex)
            {
                throw DanException.FromApiException(ex);
            }
        }

        /// <inheritdoc />
        public async Task<Accreditation> CreateDataSetRequest(List<DataSetRequest> dataSetRequests, string subject,
            string requestor = null, string consentReference = null, string consentRedirectUrl = null)
        {
            var authorizationRequest = new AuthorizationRequest()
            {
                DataSetRequests = dataSetRequests,
                Subject = subject,
                Requestor = requestor,
                ConsentReference = consentReference,
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

        /// <inheritdoc />
        public async Task<DataSet> GetDataSetFromAccreditation(
            string accreditationguid,
            string datasetname,
            TokenOnBehalfOf? tokenOnBehalfOf = null,
            bool reuseToken = false,
            string forwardAccessToken = null)
        {
            try
            {
                return await _danApi.GetEvidence(
                    accreditationguid,
                    datasetname,
                    tokenOnBehalfOf,
                    reuseToken,
                    forwardAccessToken);
            }
            catch (ApiException ex)
            {
                throw DanException.FromApiException(ex);
            }
        }

        /// <inheritdoc />
        public async Task<T> GetDataSetFromAccreditation<T>(
            string accreditationguid,
            string datasetname,
            string deserializeField = null,
            TokenOnBehalfOf? tokenOnBehalfOf = null,
            bool reuseToken = false,
            string forwardAccessToken = null) where T : new()
        {
            try
            {
                var result = await GetDataSetFromAccreditation(
                    accreditationguid,
                    datasetname,
                    tokenOnBehalfOf,
                    reuseToken,
                    forwardAccessToken);
                return GetDataSetAsTyped<T>(result, deserializeField);
            }
            catch (ApiException ex)
            {
                throw DanException.FromApiException(ex);
            }
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <summary>
        /// Attempts to deserialze the dataset to the supplied type parameter, using the deserializer supplied
        /// in the configuration
        /// </summary>
        /// <typeparam name="T">The type to deserialize to</typeparam>
        /// <param name="dataSet">The dataset containing a field to deserialize</param>
        /// <param name="deserializeField">Optionally the field in the dataset to deserialize. If not supplied, use the first.</param>
        /// <returns>The result as an instance of the supplied type.</returns>
        /// <exception cref="DanException"></exception>
        private T GetDataSetAsTyped<T>(DataSet dataSet, string deserializeField) where T : new()
        {
            try
            {
                // If deserializeField is supplied, attempt to deserialize that and ignore everything else
                if (deserializeField != null)
                {
                    var deserializeDataSetValue =
                        dataSet.Values.FirstOrDefault(x => x.Name == deserializeField);
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
                var firstDataSetValue = dataSet.Values.First();
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