using System;
using Altinn.ApiClients.Dan.Handlers;
using Altinn.ApiClients.Dan.Interfaces;
using Altinn.ApiClients.Dan.Models;
using Altinn.ApiClients.Dan.Services;
using Altinn.ApiClients.Maskinporten.Handlers;
using Altinn.ApiClients.Maskinporten.Interfaces;
using Altinn.ApiClients.Maskinporten.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Refit;

namespace Altinn.ApiClients.Dan.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDanClient<T>(this IServiceCollection services) where T : class, IClientDefinition
        {
            services.TryAddSingleton<IMemoryCache, MemoryCache>();
            services.AddHttpClient();
            services.TryAddSingleton<T>();
            services.TryAddSingleton<IDanClient, DanClient>();
            services.TryAddSingleton<IMaskinportenService, MaskinportenService>();
            services.TryAddSingleton<MaskinportenTokenHandler<T>>();

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

        public static void AddDanClientWithAccessTokenRetriever<T>(this IServiceCollection services) where T : class, IAccessTokenRetriever
        {
            services.TryAddSingleton<IMemoryCache, MemoryCache>();
            services.AddHttpClient();
            services.TryAddSingleton<IDanClient, DanClient>();
            services.TryAddSingleton<IAccessTokenRetriever, T>();
            services.TryAddTransient<AccessTokenRetrieverHandler>();

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
                }).AddHttpMessageHandler<AccessTokenRetrieverHandler>();
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