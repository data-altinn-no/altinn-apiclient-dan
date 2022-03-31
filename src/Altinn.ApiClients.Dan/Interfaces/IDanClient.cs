using System.Collections.Generic;
using System.Threading.Tasks;
using Altinn.ApiClients.Dan.Models;
using Altinn.ApiClients.Dan.Models.Enums;

namespace Altinn.ApiClients.Dan.Interfaces
{
    /// <summary>
    /// Interface for the DAN client for fetching datasets from data.altinn.no
    /// </summary>
    public interface IDanClient
    {
        /// <summary>
        /// Gets or sets the configuration object for this DAN instance.
        /// </summary>
        public IDanConfiguration Configuration { get; set; }


        /// <summary>
        /// Gets a dataset directly without a pre-existing accreditation
        /// </summary>
        /// <param name="dataSetName">The name of the dataset to request</param>
        /// <param name="subject">The party data is requested for</param>
        /// <param name="requestor">The party requesting data. If not set, will use authenticated party.</param>
        /// <param name="parameters">Any query parameters the dataset supports/requires</param>
        /// <param name="tokenOnBehalfOf">If set, will attempt to get supplier access token onbehalf of supplied party</param>
        /// <param name="reuseToken">If true, will re-use access token supplied to DAN against the dataset source. Overrides <c>tokenOnBehalfOf</c></param>
        /// <param name="forwardAccessToken">If set, will use the supplied value as the access token against the dataset source. Overrides <c>tokenOnBehalfOf</c> and <c>reuseToken</c></param>
        /// <returns>The dataset requested as an iterable dictionary</returns>
        Task<DataSet> GetDataSet(
            string dataSetName,
            string subject,
            string requestor = null,
            Dictionary<string, string> parameters = null,
            TokenOnBehalfOf? tokenOnBehalfOf = null,
            bool reuseToken = false,
            string forwardAccessToken = null);

        /// <summary>
        /// Gets a dataset directly without a pre-existing accreditation, deserializing a single field in the
        /// dataset to a model. This field will have to be of type "JsonSchema". Will unless <c>deserializeField</c>
        /// is supplied, attempt to deserialize the first field in the dataset.
        /// </summary>
        /// <typeparam name="T">The model the result should deserialize to</typeparam>
        /// <param name="dataSetName">The name of the dataset to request</param>
        /// <param name="subject">The party data is requested for</param>
        /// <param name="requestor">The party requesting data. If not set, will use authenticated party.</param>
        /// <param name="parameters">Any query parameters the dataset supports/requires</param>
        /// <param name="deserializeField">Specifies what field in the dataset to deserialize. If not supplied, the first field will be used</param>
        /// <param name="tokenOnBehalfOf">If set, will attempt to get supplier access token onbehalf of supplied party</param>
        /// <param name="reuseToken">If true, will re-use access token supplied to DAN against the dataset source. Overrides <c>tokenOnBehalfOf</c></param>
        /// <param name="forwardAccessToken">If set, will use the supplied value as the access token against the dataset source. Overrides <c>tokenOnBehalfOf</c> and <c>reuseToken</c></param>
        /// <returns>The dataset mapped to the supplied model</returns>
        Task<T> GetDataSet<T>(
            string dataSetName,
            string subject,
            string requestor = null,
            Dictionary<string, string> parameters = null,
            string deserializeField = null,
            TokenOnBehalfOf? tokenOnBehalfOf = null,
            bool reuseToken = false,
            string forwardAccessToken = null) where T : new();

        /// <summary>
        /// Creates a dataset request to the supplied list of datasets and optional parameters.
        /// </summary>
        /// <param name="dataSetRequest">The datasets requested.</param>
        /// <param name="subject">The subject of the datasets (typically norwegian organization number or SSN).</param>
        /// <param name="requestor">The party requesting the data. If not supplied will assume authenticated party</param>
        /// <param name="consentReference">The consent reference used in notifying the subject. Only used if any of the dataasets require consent from the subject</param>
        /// <param name="consentRedirectUrl">The consent redirect URL. Supply to override the default receipt page the subject is sent to after answering the consent request</param>
        /// <returns>The accreditation object</returns>
        Task<Accreditation> CreateDataSetRequest(List<DataSetRequest> dataSetRequest, string subject,
            string requestor = null, string consentReference = null, string consentRedirectUrl = null);

        /// <summary>
        /// Gets a dataset associated with an existing accreditation
        /// </summary>
        /// <param name="accreditationGuid">The id of the accreditation</param>
        /// <param name="datasetname">The name of the dataset to get data from</param>
        /// <param name="tokenOnBehalfOf">If set, will attempt to get supplier access token onbehalf of supplied party</param>
        /// <param name="reuseToken">If true, will re-use access token supplied to DAN against the dataset source. Overrides <c>tokenOnBehalfOf</c></param>
        /// <param name="forwardAccessToken">If set, will use the supplied value as the access token against the dataset source. Overrides <c>tokenOnBehalfOf</c> and <c>reuseToken</c></param>
        /// <returns>The dataset requested</returns>
        /// <exception cref="DanException"></exception>
        Task<DataSet> GetDataSetFromAccreditation(
            string accreditationGuid,
            string datasetname,
            TokenOnBehalfOf? tokenOnBehalfOf = null,
            bool reuseToken = false,
            string forwardAccessToken = null);

        /// <summary>
        /// Gets a dataset associated with an existing accreditation, deserializing a single field in the
        /// dataset to a model. This field will have to be of type "JsonSchema". Will unless <c>deserializeField</c>
        /// is supplied, attempt to deserialize the first field in the dataset.
        /// </summary>
        /// <typeparam name="T">The model the result should deserialize to</typeparam>
        /// <param name="accreditationGuid">The id of the accreditation</param>
        /// <param name="datasetname">The name of the dataset to get data from</param>
        /// <param name="deserializeField">Specifies what field in the dataset to deserialize. If not supplied, the first field will be used</param>
        /// <param name="tokenOnBehalfOf">If set, will attempt to get supplier access token onbehalf of supplied party</param>
        /// <param name="reuseToken">If true, will re-use access token supplied to DAN against the dataset source. Overrides <c>tokenOnBehalfOf</c></param>
        /// <param name="forwardAccessToken">If set, will use the supplied value as the access token against the dataset source. Overrides <c>tokenOnBehalfOf</c> and <c>reuseToken</c></param>
        /// <returns>The dataset mapped to the supplied model</returns>
        /// <exception cref="DanException"></exception>
        Task<T> GetDataSetFromAccreditation<T>(
            string accreditationGuid,
            string datasetname,
            string deserializeField = null,
            TokenOnBehalfOf? tokenOnBehalfOf = null,
            bool reuseToken = false,
            string forwardAccessToken = null) where T : new();

        /// <summary>
        /// Gets the request status for a datasets matching the supplied name within the supplied accreditation
        /// </summary>
        /// <param name="accreditationGuid">The accreditation unique identifier.</param>
        /// <param name="dataSetName">Name of the dataset.</param>
        /// <returns>The request status for all datasets matching the supplied name</returns>
        /// <exception cref="DanException"></exception>
        Task<List<DataSetRequestStatus>> GetRequestStatus(string accreditationGuid, string dataSetName);

        /// <summary>
        /// Gets the status of all datasets requested in accreditation
        /// </summary>
        /// <param name="accreditationGuid">The accreditation unique identifier.</param>
        /// <returns>The request status for all datasets</returns>
        /// <exception cref="DanException"></exception>
        Task<List<DataSetRequestStatus>> GetRequestStatus(string accreditationGuid);
    }
}