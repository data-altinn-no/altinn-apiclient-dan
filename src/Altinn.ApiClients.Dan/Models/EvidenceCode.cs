using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Altinn.ApiClients.Dan.Models
{

    /// <summary>
    /// Describing an EvidenceCode and what values it carries. When used in context of a Accreditation, also includes the timespan of which the evidence is available
    /// </summary>
    public class EvidenceCode
    {
        /// <summary>
        /// Name of the dataset
        /// </summary>
        [Required]
        [JsonPropertyName("evidenceCodeName")]
        public string EvidenceCodeName { get; set; } = string.Empty;

        /// <summary>
        /// Arbitrary text describing the purpose and content of the dataset
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>
        /// If the dataset has any parameters
        /// </summary>
        [JsonPropertyName("parameters")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<EvidenceParameter> Parameters { get; set; }

        /// <summary>
        /// Whether or not the dataset has been flagged as representing data not available by simple lookup.
        /// This causes Core to perform an explicit call to the harvester function of the dataset to 
        /// initialize or check for status.
        /// </summary>
        [JsonPropertyName("isAsynchronous")]
        public bool IsAsynchronous { get; set; }

        /// <summary>
        /// If set, specifies the maximum amount of days an accreditation referring this dataset can be valid,
        /// which also affects the duration of consent delegations and thus token expiry.
        /// </summary>
        [JsonPropertyName("maxValidDays")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int MaxValidDays { get; set; }

        /// <summary>
        /// The values associated with this dataset
        /// </summary>
        [Required]
        [JsonPropertyName("values")]
        public List<EvidenceValue> Values { get; set; } = new();

        /// <summary>
        /// The plugin the dataset belongs to
        /// </summary>
        [JsonPropertyName("evidenceSource")]
        public string EvidenceSource { get; set; } = string.Empty;

        /// <summary>
        /// The Service Code
        /// </summary>
        [JsonPropertyName("serviceCode")]
        public string ServiceCode { get; set; }

        /// <summary>
        /// The service edition code
        /// </summary>
        [JsonPropertyName("serviceEditionCode")]
        public int ServiceEditionCode { get; set; }

        /// <summary>
        /// A list of authorization requirements for the dataset, who, what, how
        /// </summary>
        [JsonPropertyName("authorizationRequirements")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<Requirement> AuthorizationRequirements { get; set; } = new();

        /// <summary>
        /// A space separated list of scopes to request when generating access tokens
        /// </summary>
        [JsonPropertyName("requiredScopes")]
        public string RequiredScopes { get; set; }

        /// <summary>
        /// DEPRECATED: An identifier of the domain service to which the dataset belongs. Use BelongsToServiceContexts
        /// </summary>
        /// <see cref="BelongsToServiceContexts"/>
        [JsonPropertyName("serviceContext")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string ServiceContext { get; set; }

        /// <summary>
        /// A list of identifiers of the domain services to which the dataset belongs
        /// </summary>
        [JsonPropertyName("belongsToServiceContexts")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string> BelongsToServiceContexts { get; set; } = new();

        /// <summary>
        /// Whether or not the evidence code has been flagged as open data.
        /// This allows it to be used without authentication, authorization and apikey
        /// </summary>
        [JsonPropertyName("isPublic")]
        public bool IsPublic { get; set; }

        /// <summary>
        /// Sets a date for when the dataset will no longer be valid.
        /// After this date the dataset will not be included in the metadata for a plugin
        /// </summary>
        [JsonPropertyName("validTo")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DateTime? ValidTo { get; set; }

        /// <summary>
        /// Specifies the date when the dataset becomes valid
        /// Prior to this date the dataset will not be included in the metadata for a plugin
        /// </summary>
        [JsonPropertyName("validFrom")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public DateTime ValidFrom { get; set; }

        /// <summary>
        /// Optional warning for datasets that will be removed in the future
        /// Should be used in combination with <see cref="ValidTo" />
        /// </summary>
        [JsonPropertyName("deprecationWarning")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string DeprecationWarning { get; set; }

        /// <summary>
        /// Optional setting for custom timeout (in seconds) in evidencecodes when harvesting data
        /// </summary>
        [JsonPropertyName("timeout")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Timeout { get; set; }
    }
}