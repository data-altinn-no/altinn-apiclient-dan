using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Altinn.ApiClients.Dan.Models.Enums;

namespace Altinn.ApiClients.Dan.Models
{
    /// <summary>
    /// Model for holding the legal basis (ESPD)
    /// </summary>
    public class LegalBasis
    {
        /// <summary>
        /// The content for the legal basis, usually the ESPD XML
        /// </summary>
        [Required]
        [JsonPropertyName("content")]
        public string Content { get; set; }

        /// <summary>
        /// Gets the arbitrary identifier for the legal basis, used to reference this from dataset requests
        /// </summary>
        [Required]
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// The type of legal basis, usually ESPD
        /// </summary>
        [Required]
        [JsonPropertyName("type")]
        public LegalBasisType? Type { get; set; }
    }
}