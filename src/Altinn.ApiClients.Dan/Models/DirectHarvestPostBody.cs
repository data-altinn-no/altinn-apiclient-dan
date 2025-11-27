using System.Text.Json.Serialization;

namespace Altinn.ApiClients.Dan.Models
{
    /// <summary>
    /// Dataset request as part of an Authorization model
    /// </summary>
    public class DirectHarvestPostBody
    {
        /// <summary>
        /// The dataset requested
        /// </summary>
        [JsonPropertyName("subject")]
        public string Subject { get; set; }
    }
}