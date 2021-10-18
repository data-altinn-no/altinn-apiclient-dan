using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Altinn.ApiClients.Dan.Models.Enums;

namespace Altinn.ApiClients.Dan.Models
{
    /// <summary>
    /// Describing the availability status of the given dataset used in the context of a Accreditation
    /// </summary>
    public class DataSetRequestStatusCode
    {
        /// <summary>
        /// The requested information can be harvested at any time
        /// </summary>
        public static DataSetRequestStatusCode Available => new DataSetRequestStatusCode()
            { Code = (int)StatusCodeId.Available, Description = "The information is available for harvest" };

        /// <summary>
        /// The requested information is not available pending a consent request in Altinn
        /// </summary>
        public static DataSetRequestStatusCode PendingConsent => new DataSetRequestStatusCode()
        {
            Code = (int)StatusCodeId.PendingConsent, Description = "Awaiting consent from subject entity representative"
        };

        /// <summary>
        /// The requested information is not available due to a consent request being denied or an existing consent has been revoked
        /// </summary>
        public static DataSetRequestStatusCode Denied => new DataSetRequestStatusCode()
            { Code = (int)StatusCodeId.Denied, Description = "Consent request denied" };

        /// <summary>
        /// The requested information is not available due to a consent delegation being expired
        /// </summary>
        public static DataSetRequestStatusCode Expired => new DataSetRequestStatusCode()
            { Code = (int)StatusCodeId.Expired, Description = "Consent expired" };

        /// <summary>
        /// The requested information is not yet available from the asynchronous source
        /// </summary>
        public static DataSetRequestStatusCode Waiting => new DataSetRequestStatusCode()
            { Code = (int)StatusCodeId.Waiting, Description = "Awaiting data from source" };

        /// <summary>
        /// The requested information is not yet available from the asynchronous source
        /// </summary>
        public static DataSetRequestStatusCode AggregateUnknown => new DataSetRequestStatusCode()
        {
            Code = (int)StatusCodeId.AggregateUnknown,
            Description =
                "The aggredate dataset status is unknown due to one or more asynchronous dataset codes in the accreditation. See /status/{accreditationId} for asynchronous dataset code status"
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