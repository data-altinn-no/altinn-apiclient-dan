using System.Text.Json;
using Altinn.ApiClients.Dan.Interfaces;

namespace Altinn.ApiClients.Dan.Models
{
    public class SystemTextJsonDeserializer : IDanDeserializer
    {
        public JsonSerializerOptions SerializerOptions { get; set; }

        public T Deserialize<T>(string json) where T : new()
        {
            return SerializerOptions != null 
                ? JsonSerializer.Deserialize<T>(json, SerializerOptions) 
                : JsonSerializer.Deserialize<T>(json);
        }
    }
}
