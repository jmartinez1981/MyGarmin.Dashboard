using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyGarmin.Connectivity.Client;
using MyGarmin.Dashboard.Api.Middlewares.ErrorHandling;
using MyGarmin.Dashboard.Api.Middlewares.RequestAndResponse;
using MyGarmin.Dashboard.Api.Settings;
using System;
using System.Net;
using System.Net.Http;

namespace MyGarmin.Dashboard.Api.Hosting
{
    /// <summary>
    /// Startup class to configure WebHost.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">Represents a set of key/value application configuration properties.</param>
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        /// <summary>
        /// Gets application configuration.
        /// </summary>
        protected IConfiguration Configuration { get; }

        /// <summary>
        /// Configure web host services.
        /// </summary>
        /// <param name="services">Service collection descriptors.</param>
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.Configure<ServerSettings>(this.Configuration.GetSection("AppSettings:Server"));
            services.Configure<GarminConnectionSettings>(this.Configuration.GetSection("AppSettings:GarminConnection"));

            services.Configure<HostOptions>(
                    opts => opts.ShutdownTimeout = this.Configuration.GetValue<TimeSpan>("ServerSettings:ShutdownTimeout"));

            services.AddHttpClient<IGarminClient, GarminClient>()
                .ConfigurePrimaryHttpMessageHandler(() => ConfigureMessageHandler());
        }

        /// <summary>
        /// Configure application and environment.
        /// </summary>
        /// <param name="app">Configure application request pipeline.</param>
        /// <param name="lifetime">Application lifetime.</param>
        /// <param name="logger">ILogger of Startup.</param>
        public virtual void Configure(
            IApplicationBuilder app,
            IHostApplicationLifetime lifetime,
            ILogger<Startup> logger)
        {
            // Register a callback to run after the app is fuly configured
            lifetime?.ApplicationStarted.Register(
              () => LogAddresses(app.ServerFeatures, logger));

            app.UseRouting();
            
            app.ConfigureRequestResponseLogging();
            app.ConfigureExceptionHandler();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void LogAddresses(IFeatureCollection features, ILogger logger)
        {
            var addressFeature = features.Get<IServerAddressesFeature>();

            if (addressFeature != null)
            {
                foreach (var addresses in addressFeature.Addresses)
                {
                    logger.LogInformation("Listening on address: " + addresses);
                }
            }
        }

        private static HttpClientHandler ConfigureMessageHandler()
        {
            return new HttpClientHandler
            {
                AllowAutoRedirect = true,
                UseCookies = true,
                CookieContainer = new CookieContainer()
            };
        }
    }
}
