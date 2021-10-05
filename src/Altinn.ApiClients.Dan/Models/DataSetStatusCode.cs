using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Altinn.ApiClients.Dan.Models.Enums;

namespace Altinn.ApiClients.Dan.Models
{
    /// <summary>
    /// Describing the availability status of the given evidence code used in the context of a Accreditation
    /// </summary>
    public class DataSetStatusCode
    {
        /// <summary>
        /// The requested information can be harvested at any time
        /// </summary>
        public static DataSetStatusCode Available => new DataSetStatusCode()
            { Code = (int)StatusCodeId.Available, Description = "The information is available for harvest" };

        /// <summary>
        /// The requested information is not available pending a consent request in Altinn
        /// </summary>
        public static DataSetStatusCode PendingConsent => new DataSetStatusCode()
        {
            Code = (int)StatusCodeId.PendingConsent, Description = "Awaiting consent from subject entity representative"
        };

        /// <summary>
        /// The requested information is not available due to a consent request being denied or an existing consent has been revoked
        /// </summary>
        public static DataSetStatusCode Denied => new DataSetStatusCode()
            { Code = (int)StatusCodeId.Denied, Description = "Consent request denied" };

        /// <summary>
        /// The requested information is not available due to a consent delegation being expired
        /// </summary>
        public static DataSetStatusCode Expired => new DataSetStatusCode()
            { Code = (int)StatusCodeId.Expired, Description = "Consent expired" };

        /// <summary>
        /// The requested information is not yet available from the asynchronous source
        /// </summary>
        public static DataSetStatusCode Waiting => new DataSetStatusCode()
            { Code = (int)StatusCodeId.Waiting, Description = "Awaiting data from source" };

        /// <summary>
        /// The requested information is not yet available from the asynchronous source
        /// </summary>
        public static DataSetStatusCode AggregateUnknown => new DataSetStatusCode()
        {
            Code = (int)StatusCodeId.AggregateUnknown,
            Description =
                "The aggredate evidence status is unknown due to one or more asynchronous evidence codes in the accreditation. See /evidence/{accreditationId} for asynchronous evidence code status"
        };

        /// <summary>
        /// Status code
        /// </summary>
        [Required]
        [JsonPropertyName("code")]
        public int? Code { get; set; }

        /// <summary>
        /// Description of the status code
        /// </summary>
        [Required]
        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>
        /// For asynchronous data sources, might include a hint at which point another attempt should be made to see if the information requested is available
        /// </summary>
        [Required]
        [JsonPropertyName("retryAt")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DateTime? RetryAt { get; set; }
    }
}