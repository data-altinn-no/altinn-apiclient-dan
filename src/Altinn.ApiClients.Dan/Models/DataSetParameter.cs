using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Altinn.ApiClients.Dan.Models.Enums;

namespace Altinn.ApiClients.Dan.Models
{
    /// <summary>
    /// Describing the format and containing the value of a dataset
    /// </summary>
    public class DataSetParameter
    {
        /// <summary>
        /// A name describing the dataset parameter
        /// </summary>
        [Required]
        [JsonPropertyName("evidenceParamName")]
        public string DataSetParamName { get; set; }

        /// <summary>
        /// The value for the dataset parameter, if used in context of a dataset request
        /// </summary>
        [JsonPropertyName("value")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public object Value { get; set; }

        /// <summary>
        /// The format of the dataset parameter
        /// </summary>
        [JsonPropertyName("paramType")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DataSetParamType? ParamType { get; set; }
    }
}