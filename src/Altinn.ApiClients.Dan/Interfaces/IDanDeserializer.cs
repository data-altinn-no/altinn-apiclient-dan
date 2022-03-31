namespace Altinn.ApiClients.Dan.Interfaces
{
    public interface IDanDeserializer
    {
        T Deserialize<T>(string json) where T : new();
    }
}
