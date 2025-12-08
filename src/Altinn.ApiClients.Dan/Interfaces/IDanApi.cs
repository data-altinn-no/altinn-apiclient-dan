using System.Collections.Generic;
using System.Threading.Tasks;
using Altinn.ApiClients.Dan.Models;
using Refit;

namespace Altinn.ApiClients.Dan.Interfaces
{
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface IDanApi
    {
        /// <summary>
        /// Gets a dataset directly without a pre-existing accreditation
        /// </summary>
        /// <param name="evidenceCode">The name of the dataset to request</param>
        /// <param name="subject">The party data is requested for</param>
        /// <param name="requestor">The party requesting data. If not set, will use authenticated party.</param>
        /// <param name="parameters">Any query parameters the dataset supports/requires</param>
        /// <param name="tokenOnBehalfOfOwner">If set, will attempt to get supplier access token onbehalf of the authenticated party</param>
        /// <param name="reuseToken">If true, will re-use access token supplied to DAN against the dataset source. Overrides <c>tokenOnBehalfOfOwner</c></param>
        /// <param name="forwardAccessToken">If set, will use the supplied value as the access token against the dataset source. Overrides <c>tokenOnBehalfOfOwner</c> and <c>reuseToken</c></param>
        /// <returns>The dataset requested</returns>
        [Get("/directharvest/{evidenceCode}")]
        Task<DataSet> GetDirectharvest(
            string evidenceCode,
            string subject,
            string requestor = null,
            Dictionary<string, string> parameters = null,
            bool tokenOnBehalfOfOwner = false,
            bool reuseToken = false,
            [Header("X-Forward-Access-Token")] string forwardAccessToken = null);

        /// <summary>
        /// Gets a dataset directly without a pre-existing accreditation with subject in body to avoid logging of NiN
        /// </summary>
        /// <param name="body">Input model containing the subject identifier</param>
        /// <param name="evidenceCode">The name of the dataset to request</param>
        /// <param name="requestor">The party requesting data. If not set, will use authenticated party.</param>
        /// <param name="parameters">Any query parameters the dataset supports/requires</param>
        /// <param name="tokenOnBehalfOfOwner">If set, will attempt to get supplier access token onbehalf of the authenticated party</param>
        /// <param name="reuseToken">If true, will re-use access token supplied to DAN against the dataset source. Overrides <c>tokenOnBehalfOfOwner</c></param>
        /// <param name="forwardAccessToken">If set, will use the supplied value as the access token against the dataset source. Overrides <c>tokenOnBehalfOfOwner</c> and <c>reuseToken</c></param>
        /// <returns>The dataset requested</returns>
        [Post("/directharvest/{evidenceCode}")]
        Task<DataSet> PostDirectharvest(
            [Body(buffered: true)] DirectHarvestPostBody body,
            string evidenceCode,
            string requestor = null,
            Dictionary<string, string> parameters = null,
            bool tokenOnBehalfOfOwner = false,
            bool reuseToken = false,
            [Header("X-Forward-Access-Token")] string forwardAccessToken = null);

        /// <summary>
        /// Gets a dataset directly without a pre-existing accreditation, not using a response envelope
        /// </summary>
        /// <param name="evidenceCode">The name of the dataset to request</param>
        /// <param name="subject">The party data is requested for</param>
        /// <param name="requestor">The party requesting data. If not set, will use authenticated party.</param>
        /// <param name="parameters">Any query parameters the dataset supports/requires</param>
        /// <param name="tokenOnBehalfOfOwner">If set, will attempt to get supplier access token onbehalf of the authenticated party</param>
        /// <param name="reuseToken">If true, will re-use access token supplied to DAN against the dataset source. Overrides <c>tokenOnBehalfOfOwner</c></param>
        /// <param name="forwardAccessToken">If set, will use the supplied value as the access token against the dataset source. Overrides <c>tokenOnBehalfOfOwner</c> and <c>reuseToken</c></param>
        /// <param name="query">Optional JMESPath query to filter/transform the dataset</param>
        /// <returns>The dataset requested as a JSON string</returns>
        [Get("/directharvest/{evidenceCode}?envelope=false")]
        Task<string> GetDirectharvestUnenveloped(
            string evidenceCode,
            string subject,
            string requestor = null,
            Dictionary<string, string> parameters = null,
            bool tokenOnBehalfOfOwner = false,
            bool reuseToken = false,
            [Header("X-Forward-Access-Token")] string forwardAccessToken = null,
            string query = null);

        /// <summary>
        /// Gets a dataset directly without a pre-existing accreditation, not using a response envelope, but with subject in body to avoid logging of NiN
        /// </summary>
        /// <param name="body">Input model containing the subject identifier</param>
        /// <param name="evidenceCode">The name of the dataset to request</param>        
        /// <param name="requestor">The party requesting data. If not set, will use authenticated party.</param>
        /// <param name="parameters">Any query parameters the dataset supports/requires</param>
        /// <param name="tokenOnBehalfOfOwner">If set, will attempt to get supplier access token onbehalf of the authenticated party</param>
        /// <param name="reuseToken">If true, will re-use access token supplied to DAN against the dataset source. Overrides <c>tokenOnBehalfOfOwner</c></param>
        /// <param name="forwardAccessToken">If set, will use the supplied value as the access token against the dataset source. Overrides <c>tokenOnBehalfOfOwner</c> and <c>reuseToken</c></param>
        /// <param name="query">Optional JMESPath query to filter/transform the dataset</param>
        /// <returns>The dataset requested as a JSON string</returns>
        [Post("/directharvest/{evidenceCode}?envelope=false")]
        [Headers("Content-Type: application/json")]
        Task<string> PostDirectharvestUnenveloped(
            [Body(buffered: true)] DirectHarvestPostBody body,
            string evidenceCode,           
            string requestor = null,
            Dictionary<string, string> parameters = null,
            bool tokenOnBehalfOfOwner = false,
            bool reuseToken = false,
            [Header("X-Forward-Access-Token")] string forwardAccessToken = null,
            string query = null);

        /// <summary>
        /// Gets a dataset associated with an existing accreditation
        /// </summary>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="accreditationId">The id of the accreditation</param>
        /// <param name="evidenceCode">The name of the dataset to get data from</param>
        /// <param name="tokenOnBehalfOfOwner">If set, will attempt to get supplier access token onbehalf of the authenticated party</param>
        /// <param name="reuseToken">If true, will re-use access token supplied to DAN against the dataset source. Overrides <c>tokenOnBehalfOfOwner</c></param>
        /// <param name="forwardAccessToken">If set, will use the supplied value as the access token against the dataset source. Overrides <c>tokenOnBehalfOfOwner</c> and <c>reuseToken</c></param>
        /// <param name="useEnvelope">If false, will attempt to unwrap the fields in the dataset to a single object for easier mapping to strong types.</param>
        /// <returns>The dataset requested</returns>
        [Get("/evidence/{accreditationId}/{evidenceCode}")]
        Task<DataSet> GetEvidence(
            string accreditationId,
            string evidenceCode,
            bool tokenOnBehalfOfOwner = false,
            bool reuseToken = false,
            [Header("X-Forward-Access-Token")] string forwardAccessToken = null,
            bool useEnvelope = true);

        /// <summary>
        /// Gets a dataset associated with an existing accreditation, not using a response envelope
        /// </summary>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="accreditationId">The id of the accreditation</param>
        /// <param name="evidenceCode">The name of the dataset to get data from</param>
        /// <param name="tokenOnBehalfOfOwner">If set, will attempt to get supplier access token onbehalf of the authenticated party</param>
        /// <param name="reuseToken">If true, will re-use access token supplied to DAN against the dataset source. Overrides <c>tokenOnBehalfOfOwner</c></param>
        /// <param name="forwardAccessToken">If set, will use the supplied value as the access token against the dataset source. Overrides <c>tokenOnBehalfOfOwner</c> and <c>reuseToken</c></param>
        /// <param name="query">Optional JMESPath query to filter/transform the dataset</param>
        /// <returns>The dataset requested as a JSON string</returns>
        [Get("/evidence/{accreditationId}/{evidenceCode}?envelope=false")]
        Task<string> GetEvidenceUnenveloped(
            string accreditationId,
            string evidenceCode,
            bool tokenOnBehalfOfOwner = false,
            bool reuseToken = false,
            [Header("X-Forward-Access-Token")] string forwardAccessToken = null,
            string query = null);

        /// <summary>
        /// Gets the status of all datasets requested in accreditation
        /// </summary>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="accreditationId">The authorized accreditation id</param>
        /// <returns>Task of void</returns>
        [Get("/evidence/{accreditationId}")]
        Task<List<DataSetRequestStatus>> GetEvidenceStatus(string accreditationId);

        /// <summary>
        /// Authorizes and creates an accreditation
        /// </summary>
        /// <param name="authorizationRequest">Post an authorization request with one or more datasets and parameters (optional)</param>
        /// <returns>The accreditation object that was created</returns>
        [Post("/authorization")]
        [Headers("Content-Type: application/json")]
        Task<Accreditation> PostAuthorization([Body(buffered: true)] AuthorizationRequest authorizationRequest);

        /// <summary>
        /// Gets all available service contexts
        /// </summary>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of void</returns>
        [Get("/public/metadata/servicecontexts")]
        Task<List<string>> GetAllServiceContexts();

        /// <summary>
        /// Gets all available datasets
        /// </summary>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="serviceContext">Optional service context for dataset</param>
        /// <returns>Task of void</returns>
        [Get("/public/metadata/evidencecodes/{serviceContext}")]
        Task<List<DataSetDefinition>> GetAllDatasets(string serviceContext = null);
    }
}
