using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Altinn.ApiClients.Dan.Models.Enums;

namespace Altinn.ApiClients.Dan.Models;

/// <summary>
/// Describing the format and containing the value of an evidence
/// </summary>
public class EvidenceValue : ICloneable
{
    /// <summary>
    /// If value type is attachment, this contains the MIME type (example: application/pdf)
    /// </summary>
    [JsonPropertyName("mimeType")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string MimeType { get; set; }

    /// <summary>
    /// A name describing the evidence value
    /// </summary>
    [Required]
    [JsonPropertyName("evidenceValueName")]
    public string EvidenceValueName { get; set; } = string.Empty;

    /// <summary>
    /// Arbitrary text describing the purpose and content of this specific field in the dataset
    /// </summary>
    [JsonPropertyName("description")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Description { get; set; }

    /// <summary>
    /// The source from which the evidence is harvested
    /// </summary>
    [Required]
    [JsonPropertyName("source")]
    public string Source { get; set; } = string.Empty;

    /// <summary>
    /// The time of which the evidence was collected from the source, if used in context of an Evidence
    /// </summary>
    [JsonPropertyName("timestamp")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateTime? Timestamp { get; set; }

    /// <summary>
    /// The value for the evidence, if used in context of an Evidence
    /// </summary>
    [JsonPropertyName("value")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object Value { get; set; }

    /// <summary>
    /// The format over the evidence value
    /// </summary>
    [Required]
    [JsonPropertyName("valueType")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EvidenceValueType? ValueType { get; set; }

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