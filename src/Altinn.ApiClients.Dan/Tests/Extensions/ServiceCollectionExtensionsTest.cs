using System.Net.Http;
using Altinn.ApiClients.Dan.Extensions;
using Altinn.ApiClients.Dan.Interfaces;
using Altinn.ApiClients.Dan.Models;
using Altinn.ApiClients.Dan.Services;
using Altinn.ApiClients.Maskinporten.Handlers;
using Altinn.ApiClients.Maskinporten.Interfaces;
using Altinn.ApiClients.Maskinporten.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Tests.Extensions
{
    [TestFixture]
    public class ServiceCollectionExtensionsTest
    {
        [Test]
        public void AddDanClient_ok()
        {
            var services = new ServiceCollection();

            services.AddOptions().Configure<DanSettings>(danSettings => danSettings.Environment = "dev");

            // services.AddOptions().Configure<DanSettings>(danSettings =>
            // {
            //     danSettings.Environment = "local";
            //     danSettings.SubscriptionKey = "subkey";
            // }); 

            ServiceCollectionExtensions.AddDanClient<SettingsJwkClientDefinition>(services);
            var serviceProvider = services.BuildServiceProvider();

            Assert.IsInstanceOf<MemoryCache>(serviceProvider.GetService<IMemoryCache>());
            Assert.IsInstanceOf<SettingsJwkClientDefinition>(serviceProvider.GetService<SettingsJwkClientDefinition>());
            Assert.IsInstanceOf<DanClient>(serviceProvider.GetService<IDanClient>());
            Assert.IsInstanceOf<MaskinportenService>(serviceProvider.GetService<IMaskinportenService>());
            Assert.IsInstanceOf<MaskinportenTokenHandler<SettingsJwkClientDefinition>>(serviceProvider
                .GetService<MaskinportenTokenHandler<SettingsJwkClientDefinition>>());
            Assert.IsInstanceOf<HttpClient>(serviceProvider.GetService<HttpClient>());
            Assert.NotNull(serviceProvider.GetService<IDanApi>());
        }
    }
}