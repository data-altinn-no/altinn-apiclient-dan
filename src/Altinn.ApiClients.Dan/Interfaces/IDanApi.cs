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
        /// Gets the evidence directly without a pre-existing accreditation
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="evidenceCode">The name of the evidence code</param>
        /// <param name="subject">The party data is requested for</param>
        /// <param name="requestor">The party requesting data</param>
        /// <returns>Task of void</returns>
        [Get("/directharvest/{evidenceCode}")]
        Task<DataSet> GetDirectharvest(string evidenceCode, string subject, string requestor = null, Dictionary<string, string> parameters = null);

        /// <summary>
        /// Gets the evidence
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="accreditationId">The id of the accreditation</param>
        /// <param name="evidenceCode">The name of the evidence code to get data from</param>
        /// <returns>Task of void</returns>
        [Get("/evidence/{accreditationId}/{evidenceCode}")]
        Task<DataSet> GetEvidence(string accreditationId, string evidenceCode);

        /// <summary>
        /// Gets the status of all evidence codes requested in accreditation
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="accreditationId">The authorized accreditation id</param>
        /// <returns>Task of void</returns>
        [Get("/evidence/{accreditationId}")]
        Task<List<DataSetRequestStatus>> GetEvidenceStatus(string accreditationId);

        /// <summary>
        /// Authorizes and creates an accreditation
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="authorizationRequest">Post an authorization request with one or more evidence codes and parameters (optional)</param>
        /// <returns>Task of void</returns>
        [Post("/authorization")]
        [Headers("Content-Type: application/json")]
        Task<Accreditation> PostAuthorization([Body(buffered: true)] AuthorizationRequest authorizationRequest);
    }
}