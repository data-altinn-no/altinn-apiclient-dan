using Altinn.ApiClients.Dan.Interfaces;

namespace Altinn.ApiClients.Dan.Models
{
    public class DanConfiguration : IDanConfiguration
    {
        public IDanDeserializer Deserializer { get; set; }
    }
}
