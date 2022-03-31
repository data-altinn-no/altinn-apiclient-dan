using Altinn.ApiClients.Dan.Interfaces;

namespace Altinn.ApiClients.Dan.Models
{
    public class DefaultDanConfiguration : IDanConfiguration
    {
        public IDanDeserializer Deserializer => new SystemTextJsonDeserializer();
    }
}
