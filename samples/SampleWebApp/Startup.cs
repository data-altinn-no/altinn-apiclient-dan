using Altinn.ApiClients.Dan.Extensions;
using Altinn.ApiClients.Maskinporten.Extensions;
using Altinn.ApiClients.Maskinporten.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SampleWebApp
{
    public class MyClientForDan {}

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Named
            services.AddMaskinportenHttpClient<SettingsJwkClientDefinition>("myClientForDan", Configuration.GetSection("MaskinportenSettings"));

            services
                .AddDanClient(Configuration.GetSection("DanSettings"))
                .AddMaskinportenHttpMessageHandler<SettingsJwkClientDefinition>("myClientForDan");

            // Typed
            services.AddMaskinportenHttpClient<SettingsJwkClientDefinition, MyClientForDan>(Configuration.GetSection("MaskinportenSettings"));

            services
                .AddDanClient(Configuration.GetSection("DanSettings"))
                .AddMaskinportenHttpMessageHandler<SettingsJwkClientDefinition, MyClientForDan>();

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