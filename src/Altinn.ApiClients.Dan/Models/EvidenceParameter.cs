using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Altinn.ApiClients.Dan.Models.Enums;

namespace Altinn.ApiClients.Dan.Models
{

    /// <summary>
    /// Describing the format and containing the value of an evidence
    /// </summary>
    public class EvidenceParameter
    {
        /// <summary>
        /// A name describing the evidence parameter
        /// </summary>
        [Required]
        [JsonPropertyName("evidenceParamName")]
        public string EvidenceParamName { get; set; }

        /// <summary>
        /// The format of the evidence parameter
        /// </summary>
        [JsonPropertyName("paramType")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public EvidenceParamType? ParamType { get; set; }

        /// <summary>
        /// Whether or not the evidence parameter is required, if used in context of a evidence code description
        /// </summary>
        [JsonPropertyName("required")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Required { get; set; }

        /// <summary>
        /// The value for the evidence parameter, if used in context of a evidence code request
        /// </summary>
        [JsonPropertyName("value")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public object Value { get; set; }
    }
}