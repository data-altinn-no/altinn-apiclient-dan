using System;
using Altinn.ApiClients.Dan.Interfaces;
using Altinn.ApiClients.Dan.Models;
using Altinn.ApiClients.Maskinporten.Handlers;
using Altinn.ApiClients.Maskinporten.Interfaces;
using Altinn.ApiClients.Maskinporten.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Refit;

namespace Altinn.ApiClients.Dan.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDanClient<T>(this IServiceCollection services) where T : IClientDefinition
        {
            services.TryAddSingleton<IMaskinportenService, MaskinportenService>();
            services.AddSingleton<MaskinportenTokenHandler<T>>();

            DanSettings danSettings = null;
            services.AddRefitClient<IDanApi>(sp =>
                {
                    danSettings = sp.GetRequiredService<IOptions<DanSettings>>().Value;
                    return new RefitSettings();
                })
                .ConfigureHttpClient(c =>
                {
                    c.BaseAddress = new Uri(GetUriForEnvironment(danSettings.Environment));
                    c.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", danSettings.SubscriptionKey);
                })
                .AddHttpMessageHandler<MaskinportenTokenHandler<T>>();
        }

        private static string GetUriForEnvironment(string environment)
        {
            return environment switch
            {
                "local" => "http://localhost:7071/api",
                "dev" => "https://apim-nadobe-dev.azure-api.net/v1",
                "staging" => "https://test-api.data.altinn.no/v1",
                "prod" => "https://api.data.altinn.no/v1",
                _ => throw new ArgumentException("Invalid environment setting. Valid values: local, dev, staging, prod")
            };
        }
    }
}