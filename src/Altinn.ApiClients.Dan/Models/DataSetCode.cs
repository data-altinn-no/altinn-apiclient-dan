using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Altinn.ApiClients.Dan.Models;
using Altinn.ApiClients.Dan.Models.Enums;

namespace Nadobe.Common.Models
{

    /// <summary>
    /// Describing an DataSetCode and what values it carries. When used in context of a Accreditation, also includes the timespan of which the evidence is available
    /// </summary>
    public class DataSetCode
    {
        /// <summary>
        /// Name of the evidence code
        /// </summary>
        [Required]
        [JsonPropertyName("evidenceCodeName")]
        public string DataSetName { get; set; }

        /// <summary>
        /// Arbitrary text describing the purpose and content of the evidence code
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }        

        /// <summary>
        /// How the evidence is accessed
        /// </summary>
        [Required]
        [JsonPropertyName("accessMethod")]
        // [JsonConverter(typeof(StringEnumConverter))]
        public DataSetAccessMethod? AccessMethod { get; set; }

        /// <summary>
        /// If the evidence code has any parameters
        /// </summary>
        [JsonPropertyName("parameters")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<DataSetParameter> Parameters { get; set; }

        /// <summary>
        /// Whether or not the evidence code has been flagged as representing data not available by simple lookup.
        /// This causes Core to perform an explicit call to the harvester function of the evidence source to 
        /// initialize or check for status.
        /// </summary>
        [JsonPropertyName("isAsynchronous")]
        public bool IsAsynchronous { get; set; }

        /// <summary>
        /// If set, specifies the maximum amount of days an accreditation referring this evidence code can be valid,
        /// which also affects the duration of consent delegations and thus token expiry.
        /// </summary>
        [JsonPropertyName("maxValidDays")]
        public int? MaxValidDays { get; set; }

        /// <summary>
        /// The values associated with this evidence code
        /// </summary>
        [Required]
        [JsonPropertyName("values")]
        public List<DataSetValue> Values { get; set; }

        /// <summary>
        /// A list of authorization requirements for the evidence code, who, what, how
        /// </summary>
        [JsonPropertyName("authorizationRequirements")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<Requirement> AuthorizationRequirements { get; set; }

        /// <summary>
        ///  An identifier of the domain service to which the dataset belongs
        /// </summary>
        [JsonPropertyName("serviceContext")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string ServiceContext { get; set; }
    }
}
