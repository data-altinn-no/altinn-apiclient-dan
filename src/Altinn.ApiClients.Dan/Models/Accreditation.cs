using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Nadobe.Common.Models;

namespace Altinn.ApiClients.Dan.Models
{
    /// <summary>
    /// The accreditation returned from a successful authorization
    /// </summary>
    public class Accreditation
    {
        /// <summary>
        /// Gets or sets the identifier for the accreditation
        /// </summary>
        [JsonPropertyName("id")]
        public string AccreditationId { get; set; }

        /// <summary>
        /// Gets or sets the party requesting the evidence
        /// </summary>
        [JsonPropertyName("requestor")]
        public string Requestor { get; set; }

        /// <summary>
        /// Gets or sets the party the evidence is requested for
        /// </summary>
        [Required]
        [JsonPropertyName("subject")]
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the aggregate status for all evidence codes in the accreditation, excluding asynchronous evidence codes.
        /// </summary>
        [Required]
        [JsonPropertyName("aggregateStatus")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DataSetStatusCode AggregateStatus { get; set; }

        [JsonPropertyName("isDirectHarvest")] public bool IsDirectHarvest { get; set; }

        /// <summary>
        /// Gets or sets a list of evidence codes associated with the accreditation. Only supplied when requesting a single accreditation.
        /// </summary>
        [JsonPropertyName("evidenceCodes")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<DataSetCode> DataSetCodes { get; set; }

        /// <summary>
        /// Gets or sets when the accreditation was created
        /// </summary>
        [Required]
        [JsonPropertyName("issued")]
        public DateTime? Issued { get; set; }

        /// <summary>
        /// Gets or sets when the accreditation was last changed. Usually means the time at which an consent request was answered.
        /// </summary>
        [Required]
        [JsonPropertyName("lastChanged")]
        public DateTime? LastChanged { get; set; }

        /// <summary>
        /// Gets or sets how long the accreditation is valid
        /// </summary>
        [Required]
        [JsonPropertyName("validTo")]
        public DateTime ValidTo { get; set; }

        /// <summary>
        /// Gets or sets TED reference
        /// </summary>
        [JsonPropertyName("consentReference")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string ConsentReference { get; set; }


        /// <summary>
        /// Gets or sets arbitrary reference provided in the authorization call
        /// </summary>
        [JsonPropertyName("externalReference")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string ExternalReference { get; set; }

        /// <summary>
        /// Gets or sets a list of evidenceCodes that have been verified
        /// </summary>
        [JsonPropertyName("evidenceCodesWithVerifiedLegalBasis")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string> DataSetWithVerifiedLegalBasis { get; set; }

        /// <summary>
        /// Used by the JSON serializer, causes the above field to only be included if non-empty
        /// </summary>
        /// <returns>True if the list is not empty</returns>
        public bool ShouldSerializeDataSetWithVerifiedLegalBasis()
        {
            return DataSetWithVerifiedLegalBasis?.Count > 0;
        }

        /// <summary>
        /// The selected language for the accreditation, used for consent request texts and notifications, no-nb, no-nn or en
        /// </summary>
        [JsonPropertyName("languageCode")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string LanguageCode { get; set; }
    }
}