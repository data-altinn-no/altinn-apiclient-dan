using System;
using Altinn.ApiClients.Dan.Handlers;
using Altinn.ApiClients.Dan.Interfaces;
using Altinn.ApiClients.Dan.Models;
using Altinn.ApiClients.Dan.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Refit;

namespace Altinn.ApiClients.Dan.Extensions
{
    /// <summary>
    /// Service collection extensions for DAN
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds a DAN Client to the service collection, assuming IOption&lt;DanSettings&gt; is already configured.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="danConfigurationProvider"></param>
        /// <returns></returns>
        public static IHttpClientBuilder AddDanClient(this IServiceCollection services, Func<IServiceProvider, IDanConfiguration> danConfigurationProvider = null)
            => AddDanClient(services, null, danConfigurationProvider);

        /// <summary>
        /// Adds a DAN Client to the service collection
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="configuration">The configuration object to bind to DanSettings</param>
        /// <param name="danConfigurationProvider">Provider for configuration settings</param>
        public static IHttpClientBuilder AddDanClient(
            this IServiceCollection services, 
            IConfiguration configuration,
            Func<IServiceProvider, IDanConfiguration> danConfigurationProvider = null)
        {
            if (configuration != null)
            {
                services.Configure<DanSettings>(configuration);
            }

            services.TryAddSingleton(sp => danConfigurationProvider != null
                ? danConfigurationProvider.Invoke(sp)
                : new DefaultDanConfiguration());
            services.TryAddSingleton<IDanClient, DanClient>();

            DanSettings danSettings = null;
            return services.AddRefitClient<IDanApi>(sp =>
            {
                danSettings = sp.GetRequiredService<IOptions<DanSettings>>().Value;
                return new RefitSettings();
            })
            .ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri(GetUriForEnvironment(danSettings.Environment));
                c.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", danSettings.SubscriptionKey);
            });
        }

        /// <summary>
        /// Adds a DAN Client to the service collection with a custom token retriever for when Maskinporten-client shouldn't be used
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <typeparam name="T">The implementation of IAccessTokenRetriever to be used</typeparam>
        public static IHttpClientBuilder AddDanClientWithAccessTokenRetriever<T>(this IServiceCollection services) where T : class, IAccessTokenRetriever
        {
            services.TryAddSingleton<IDanClient, DanClient>();
            services.TryAddSingleton<IAccessTokenRetriever, T>();
            services.TryAddTransient<AccessTokenRetrieverHandler>();

            DanSettings danSettings = null;
            return services.AddRefitClient<IDanApi>(sp =>
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