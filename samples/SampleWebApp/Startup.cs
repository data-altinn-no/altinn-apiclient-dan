using Altinn.ApiClients.Dan.Extensions;
using Altinn.ApiClients.Dan.Interfaces;
using Altinn.ApiClients.Dan.Models;
using Altinn.ApiClients.Maskinporten.Config;
using Altinn.ApiClients.Maskinporten.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SampleWebApp.Services;

namespace SampleWebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Configuration for DAN (environment and subscription key)
            services.Configure<DanSettings>(Configuration.GetSection("DanSettings"));


            // Config for MaskinPorten, using a local PKCS#12 file containing private certificate for signing Maskinporten requests
            services.Configure<MaskinportenSettings<Pkcs12ClientDefinition>>(Configuration.GetSection("MyMaskinportenSettingsForCertFile"));
            // This registers an IDanClient for injection, see Controllers/DanClientTestController.cs for usage example
            services.AddDanClient<Pkcs12ClientDefinition>();

            // If the secret required for Maskinporten is found elsewhere, you can either use one of the built-in client definitions, or provide your
            // own, like this
            //services.Configure<MaskinportenSettings<MyCustomDanClientDefinition>>(Configuration.GetSection("MyMaskinportenSettingsForCertFile"));
            //services.Configure<DanSettings>(Configuration.GetSection("DanSettings"));
            //services.AddDanClient<MyCustomDanClientDefinition>();

            // If you want to get the token from some other place, you can implementent your own IAccessTokenRetriever, and use this instead of Maskinporten. 
            //services.AddDanClientWithAccessTokenRetriever<MyAccessTokenRetriever>();

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}