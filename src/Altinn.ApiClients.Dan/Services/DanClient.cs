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

        /// <summary>
        /// Constructs a DAN client 
        /// </summary>
        /// <param name="danApi">The Refit implementation for the DAN API</param>
        /// <param name="danConfiguration">Optional DAN configuration object for specifying a custom deserializer</param>
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
            bool tokenOnBehalfOfOwner = false,
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
                    tokenOnBehalfOfOwner,
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
            bool tokenOnBehalfOfOwner = false,
            bool reuseToken = false,
            string forwardAccessToken = null,
            string query = null) where T : new()
        {
            try
            {
                if (deserializeField != null)
                {
                    var result = await GetDataSet(
                        dataSetName,
                        subject,
                        requestor,
                        parameters,
                        tokenOnBehalfOfOwner,
                        reuseToken,
                        forwardAccessToken);

                    return GetDataSetAsTyped<T>(result, deserializeField);
                }
                else
                {
                    var result = await _danApi.GetDirectharvestUnenveloped(
                        dataSetName,
                        subject,
                        requestor,
                        parameters,
                        tokenOnBehalfOfOwner,
                        reuseToken,
                        forwardAccessToken,
                        query);

                    return GetUnenvelopedDataSetAsTyped<T>(result);
                }

            }
            catch (ApiException ex)
            {
                throw DanException.FromApiException(ex);
            }
        }

        /// <inheritdoc />
        public async Task<Accreditation> CreateDataSetRequest(List<DataSetRequest> dataSetRequests, string subject,
            string requestor = null, string consentReference = null, string consentRedirectUrl = null, bool skipAltinnNotification = false, List<LegalBasis> legalBasisList = null)
        {
            var authorizationRequest = new AuthorizationRequest()
            {
                DataSetRequests = dataSetRequests,
                Subject = subject,
                Requestor = requestor,
                ConsentReference = consentReference,
                ConsentReceiptRedirectUrl = consentRedirectUrl,
                SkipAltinnNotification = skipAltinnNotification,
                LegalBasisList = legalBasisList
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
            bool tokenOnBehalfOfOwner = false,
            bool reuseToken = false,
            string forwardAccessToken = null)
        {
            try
            {
                return await _danApi.GetEvidence(
                    accreditationguid,
                    datasetname,
                    tokenOnBehalfOfOwner,
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
            bool tokenOnBehalfOfOwner = false,
            bool reuseToken = false,
            string forwardAccessToken = null,
            string query = null) where T : new()
        {
            try
            {
                if (deserializeField != null)
                {
                    var result = await GetDataSetFromAccreditation(
                        accreditationguid,
                        datasetname,
                        tokenOnBehalfOfOwner,
                        reuseToken,
                        forwardAccessToken);

                    return GetDataSetAsTyped<T>(result, deserializeField);
                }
                else
                {
                    var result = await _danApi.GetEvidenceUnenveloped(
                        accreditationguid,
                        datasetname,
                        tokenOnBehalfOfOwner,
                        reuseToken,
                        forwardAccessToken,
                        query);

                    return GetUnenvelopedDataSetAsTyped<T>(result);
                }
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
        /// <param name="deserializeField">The field in the dataset to deserialize.</param>
        /// <returns>The result as an instance of the supplied type.</returns>
        /// <exception cref="DanException"></exception>
        private T GetDataSetAsTyped<T>(DataSet dataSet, string deserializeField) where T : new()
        {
            if (deserializeField == null)
            {
                throw new ArgumentNullException(nameof(deserializeField));
            }

            try
            {

                var deserializeDataSetValue =
                    dataSet.Values.FirstOrDefault(x => x.Name == deserializeField);
                if (deserializeDataSetValue == null)
                {
                    throw new DanException(
                        $"Target field for deserialization '{deserializeField}' was not present in the dataset");
                }

                if (deserializeDataSetValue.ValueType != DataSetValueType.JsonSchema)
                {
                    throw new DanException(
                        $"Mapping to model '{typeof(T).Name}' requires the dataset to have a field of type 'JsonSchema', which is either the first field or specified via the 'deserializeField' parameter");
                }

                return deserializeDataSetValue.Value == null
                    ? default
                    : Configuration.Deserializer.Deserialize<T>(deserializeDataSetValue.Value.ToString()!);
            }
            catch (Exception ex)
            {
                throw new DanException("Unable to deserialize to requested type: " + ex.Message, ex);
            }
        }

        private T GetUnenvelopedDataSetAsTyped<T>(string resultJson) where T : new()
        {
            try
            {
                return Configuration.Deserializer.Deserialize<T>(resultJson);
            }
            catch (Exception ex)
            {
                throw new DanException("Unable to deserialize to requested type: " + ex.Message, ex);
            }
        }

        /// <inheritdoc />
        public async Task<List<string>> GetAllServiceContexts()
        {
            try
            {
                return await _danApi.GetAllServiceContexts();
            }
            catch (Exception ex)
            {
                throw new DanException("Test error: " + ex.Message, ex);
            }
        }

        /// <inheritdoc />
        public async Task<List<DataSetDefinition>> GetAllDatasets()
        {
            try
            {
                return await _danApi.GetAllDatasets();
            }
            catch (Exception ex)
            {
                throw new DanException("Test error: " + ex.Message, ex);
            }
        }

        /// <inheritdoc />
        public Task<List<DataSetDefinition>> GetDatasetsForServiceContext(string serviceContext)
        {
            try
            {
                return _danApi.GetAllDatasets(serviceContext);
            }
            catch (Exception ex)
            {
                throw new DanException("Test error: " + ex.Message, ex);
            }
        }
    }
}