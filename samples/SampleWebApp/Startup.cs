using Altinn.ApiClients.Dan.Extensions;
using Altinn.ApiClients.Maskinporten.Extensions;
using Altinn.ApiClients.Maskinporten.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace SampleWebApp
{
    public class Startup
    {
        private const string MyClientDefinitionForDan = "my-client-definition-for-dan";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.RegisterMaskinportenClientDefinition<SettingsJwkClientDefinition>(MyClientDefinitionForDan,
                Configuration.GetSection("MaskinportenSettingsForDanClient"));

            services
                .AddDanClient(Configuration.GetSection("DanSettings"))
                .AddMaskinportenHttpMessageHandler<SettingsJwkClientDefinition>(MyClientDefinitionForDan);

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, "Views")),
                RequestPath = "/Views"
            });
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
