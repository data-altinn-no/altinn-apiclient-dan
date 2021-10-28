using System;
using System.Threading.Tasks;
using Altinn.ApiClients.Maskinporten.Config;
using Altinn.ApiClients.Maskinporten.Interfaces;
using Altinn.ApiClients.Maskinporten.Models;
using Microsoft.Extensions.Options;

namespace SampleWebApp.Services
{
    public class MyCustomDanClientDefinition : IClientDefinition
    {
        public MaskinportenSettings ClientSettings { get; set; }

        public MyCustomDanClientDefinition(IOptions<MaskinportenSettings<MyCustomDanClientDefinition>> clientSettings)
        {
            ClientSettings = clientSettings.Value;
        }

        public MyCustomDanClientDefinition(MaskinportenSettings clientSettings)
        {
            ClientSettings = clientSettings;
        }

        public Task<ClientSecrets> GetClientSecrets()
        {
            throw new NotImplementedException();
        }
    }
}