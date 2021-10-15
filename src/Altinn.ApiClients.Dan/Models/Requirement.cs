using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Altinn.ApiClients.Dan.Models
{ 
    /// <summary>
    /// Base class for all authorization requirements
    /// </summary>
    [DataContract]
    public class Requirement
    {
        /// <summary>
        ///  Used for serializing only
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("type")]
        public string RequirementType;
    }
}
  