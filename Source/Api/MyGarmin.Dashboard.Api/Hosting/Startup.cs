using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyGarmin.Dashboard.Api.Hosting.Extensions;
using MyGarmin.Dashboard.Api.Middlewares.ErrorHandling;
using MyGarmin.Dashboard.ApplicationServices.Extensions;
using Serilog;

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

            services
                .AddHttpContextAccessor()
                .ConfigureAuthorization(this.Configuration)
                .ConfigureCors()
                .ConfigureSettings(this.Configuration)
                .ConfigureGarminClient()
                .ConfigureStravaClient();

            services
                .ConfigureApplicationServices()
                .ConfigureDataAccess(this.Configuration);
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

            app.ConfigureExceptionHandler();

            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void LogAddresses(IFeatureCollection features, Microsoft.Extensions.Logging.ILogger logger)
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

        
    }
}
