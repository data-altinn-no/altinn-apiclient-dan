using Altinn.ApiClients.Dan.Extensions;
using Altinn.ApiClients.Dan.Interfaces;
using Altinn.ApiClients.Dan.Models;
using Altinn.ApiClients.Dan.Services;
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

            services.AddDanClient();
            var serviceProvider = services.BuildServiceProvider();

            Assert.IsInstanceOf<DanClient>(serviceProvider.GetService<IDanClient>());
            Assert.NotNull(serviceProvider.GetService<IDanApi>());
        }
    }
}