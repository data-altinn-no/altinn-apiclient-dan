using System;
using System.Text.Json.Serialization;

namespace Altinn.ApiClients.Dan.Models
{
    /// <summary>
    /// Evidence Request as part of an Authorization model
    /// </summary>
    public class DataSetRequestStatus
    {
        /// <summary>
        /// The name of the evidence code this status refers to
        /// </summary>
        [JsonPropertyName("evidenceCodeName")]
        public string DataSetName { get; set; }

        /// <summary>
        /// Gets or Sets Status
        /// </summary>
        [JsonPropertyName("status")]
        public DataSetStatusCode Status { get; set; }

        /// <summary>
        /// From when the evidence code is available
        /// </summary>
        [JsonPropertyName("validFrom")]
        public DateTime? ValidFrom { get; set; }

        /// <summary>
        /// Until when the evidence code is available
        /// </summary>
        [JsonPropertyName("validTo")]
        public DateTime? ValidTo { get; set; }

        /// <summary>
        /// Whether or not the authorization included legal basis. For now, this is always ESPD
        /// </summary>
        [JsonPropertyName("didSupplyLegalBasis")]
        public bool? DidSupplyLegalBasis { get; set; }
    }
}