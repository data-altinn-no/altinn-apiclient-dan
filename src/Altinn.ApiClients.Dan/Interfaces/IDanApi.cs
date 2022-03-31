using System.Collections.Generic;
using System.Threading.Tasks;
using Altinn.ApiClients.Dan.Models;
using Altinn.ApiClients.Dan.Models.Enums;
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
        /// <param name="tokenOnBehalfOf">If set, will attempt to get supplier access token onbehalf of supplied party</param>
        /// <param name="reuseToken">If true, will re-use access token supplied to DAN against the dataset source. Overrides <c>tokenOnBehalfOf</c></param>
        /// <param name="forwardAccessToken">If set, will use the supplied value as the access token against the dataset source. Overrides <c>tokenOnBehalfOf</c> and <c>reuseToken</c></param>
        /// <returns>The dataset requested</returns>
        [Get("/directharvest/{evidenceCode}")]
        Task<DataSet> GetDirectharvest(
            string evidenceCode,
            string subject,
            string requestor = null,
            Dictionary<string, string> parameters = null,
            TokenOnBehalfOf? tokenOnBehalfOf = null,
            bool reuseToken = false,
            [Header("X-Forward-Access-Token")] string forwardAccessToken = null);

        /// <summary>
        /// Gets a dataset associated with an existing accreditation
        /// </summary>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="accreditationId">The id of the accreditation</param>
        /// <param name="evidenceCode">The name of the dataset to get data from</param>
        /// <param name="tokenOnBehalfOf">If set, will attempt to get supplier access token onbehalf of supplied party</param>
        /// <param name="reuseToken">If true, will re-use access token supplied to DAN against the dataset source. Overrides <c>tokenOnBehalfOf</c></param>
        /// <param name="forwardAccessToken">If set, will use the supplied value as the access token against the dataset source. Overrides <c>tokenOnBehalfOf</c> and <c>reuseToken</c></param>
        /// <returns>The dataset requested</returns>
        [Get("/evidence/{accreditationId}/{evidenceCode}")]
        Task<DataSet> GetEvidence(
            string accreditationId,
            string evidenceCode,
            TokenOnBehalfOf? tokenOnBehalfOf = null,
            bool reuseToken = false,
            [Header("X-Forward-Access-Token")] string forwardAccessToken = null);

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
    }
}