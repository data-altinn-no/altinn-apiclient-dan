using System.Text.Json.Serialization;

namespace Altinn.ApiClients.Dan.Models
{
    /// <summary>
    /// Request body for directharvest POST calls where the subject is sent in the body.
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