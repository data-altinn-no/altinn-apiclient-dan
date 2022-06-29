using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Altinn.ApiClients.Dan.Models
{
    /// <summary>
    /// The authorization model that must be supplied to that /authorization REST endpoint
    /// </summary>
    public class AuthorizationRequest
    {
        /// <summary>
        /// The party requesting the dataset
        /// </summary>
        [JsonPropertyName("requestor")]
        public string Requestor { get; set; }

        /// <summary>
        /// The party the dataset is requested for
        /// </summary>
        [JsonPropertyName("subject")]
        public string Subject { get; set; }

        /// <summary>
        /// The requested dataset
        /// </summary>
        [JsonPropertyName("evidenceRequests")]
        public List<DataSetRequest> DataSetRequests { get; set; }

        /// <summary>
        /// List of legal basis proving legal authority for the requested dataset
        /// </summary>
        [JsonPropertyName("legalBasisList")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<LegalBasis> LegalBasisList { get; set; }

        /// <summary>
        /// How long the accreditation should be valid. Also used for duration of consent (date part only).
        /// </summary>
        [JsonPropertyName("validTo")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DateTime? ValidTo { get; set; }

        /// <summary>
        /// TED reference number, if applicable
        /// </summary>
        [JsonPropertyName("consentReference")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string ConsentReference { get; set; }

        /// <summary>
        /// Arbitrary reference which will be saved with the Accreditation
        /// </summary>
        [JsonPropertyName("externalReference")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string ExternalReference { get; set; }

        /// <summary>
        /// The selected language for the accreditation, used for consent request texts and notifications. Valid values: no-nb, no-nn, en
        /// </summary>
        [JsonPropertyName("languageCode")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string LanguageCode { get; set; }

        /// <summary>
        /// URL for redirect from funcconsentreceipt if user is in GUI guided process
        /// </summary>
        [JsonPropertyName("consentReceiptRedirectUrl")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string ConsentReceiptRedirectUrl { get; set; }

        /// <summary>
        /// Skip sending a message to Altinn with the consent request and handling it in the client with consentReceiptRedirectUrl
        /// </summary>
        [JsonPropertyName("skipAltinnNotification")]
        public bool SkipAltinnNotification { get; set; }
    }
}