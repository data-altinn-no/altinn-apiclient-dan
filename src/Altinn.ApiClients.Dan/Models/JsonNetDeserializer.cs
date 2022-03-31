using Newtonsoft.Json;
using Altinn.ApiClients.Dan.Interfaces;

namespace Altinn.ApiClients.Dan.Models
{
    public class JsonNetDeserializer : IDanDeserializer
    {
        public JsonSerializerSettings SerializerSettings { get; set; }

        public T Deserialize<T>(string json) where T : new()
        {
            return SerializerSettings != null 
                ? JsonConvert.DeserializeObject<T>(json, SerializerSettings)
                : JsonConvert.DeserializeObject<T>(json);
        }
    }
}
