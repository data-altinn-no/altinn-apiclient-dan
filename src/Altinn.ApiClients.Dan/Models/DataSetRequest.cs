using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Altinn.ApiClients.Dan.Models
{
    /// <summary>
    /// Dataset request as part of an Authorization model
    /// </summary>
    public class DataSetRequest
    {
        /// <summary>
        /// The dataset requested
        /// </summary>
        [JsonPropertyName("evidenceCodeName")]
        public string DataSetName { get; set; }

        /// <summary>
        /// Supplied parameters
        /// </summary>
        [JsonPropertyName("parameters")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<DataSetParameter> Parameters { get; set; }

        /// <summary>
        /// If a legal basis is supplied, its identifier goes here
        /// </summary>
        [JsonPropertyName("legalBasisId")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string LegalBasisId { get; set; }

        /// <summary>
        /// If a legal basis is supplied, the reference within it may be supplied here if applicable
        /// </summary>
        [JsonPropertyName("legalBasisReference")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string LegalBasisReference { get; set; }

        /// <summary>
        /// Whether a request for non-open dataset not covered by legal basis should result in a consent request being initiated
        /// </summary>
        [JsonPropertyName("requestConsent")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? RequestConsent { get; set; }
    }
}