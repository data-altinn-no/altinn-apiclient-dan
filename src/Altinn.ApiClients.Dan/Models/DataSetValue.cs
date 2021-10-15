using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Altinn.ApiClients.Dan.Models.Enums;

namespace Altinn.ApiClients.Dan.Models
{
    /// <summary>
    /// Describing the format and containing the value of a dataset
    /// </summary>
    public class DataSetValue : ICloneable
    {
        /// <summary>
        /// If value type is attachment, this contains the MIME type (example: application/pdf)
        /// </summary>
        [JsonPropertyName("mimeType")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string MimeType { get; set; }

        /// <summary>
        /// A name describing the dataset value
        /// </summary>
        [Required]
        [JsonPropertyName("evidenceValueName")]
        public string DataSetValueName { get; set; }

        /// <summary>
        /// The source from which the dataset is harvested
        /// </summary>
        [Required]
        [JsonPropertyName("source")]
        public string Source { get; set; }

        /// <summary>
        /// The time of which the dataset was collected from the source
        /// </summary>
        [JsonPropertyName("timestamp")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DateTime? Timestamp { get; set; }

        /// <summary>
        /// The value for the dataset
        /// </summary>
        [JsonPropertyName("value")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public object Value { get; set; }

        /// <summary>
        /// The format of the dataset
        /// </summary>
        [Required]
        [JsonPropertyName("valueType")]
        public DataSetValueType? ValueType { get; set; }

        /// <summary>
        /// If a richer type is required, a JSON Schema may be supplied
        /// </summary>
        [JsonPropertyName("jsonSchemaDefintion")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string JsonSchemaDefintion { get; set; }

        /// <inheritdoc />
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}