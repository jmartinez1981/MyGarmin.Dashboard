using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MyGarmin.Connectivity.Client;
using MyGarmin.Dashboard.Api.DelegatingHandlers;
using MyGarmin.Dashboard.Api.Middlewares.ErrorHandling;
using MyGarmin.Dashboard.ApplicationServices;
using MyGarmin.Dashboard.Common.Settings;
using MyGarmin.Dashboard.Connectivity.StravaClient;
using MyGarmin.Dashboard.Connectivity.StravaClient.Uris;
using Serilog;
using System;
using System.Net;
using System.Net.Http;
using System.Text;

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
            services.Configure<AuthenticationSettings>(this.Configuration.GetSection("AppSettings:Authentication"));

            services.Configure<HostOptions>(
                    opts => opts.ShutdownTimeout = this.Configuration.GetValue<TimeSpan>("AppSettings:Server:ShutdownTimeout"));

            services.AddHttpClient<IGarminClient, GarminClient>()
                .ConfigurePrimaryHttpMessageHandler(() => ConfigureGarminClientMessageHandler());

            services.AddHttpClient<IStravaClient, StravaClient>(client =>
            {
                client.BaseAddress = ApiV3Uri.BaseApi;
            })
            .AddHttpMessageHandler<AuthenticationDelegatingHandler>();

            services.AddScoped<AuthenticationDelegatingHandler>();
            services.AddScoped<IUserService, UserService>();

            // configure jwt authentication
            var authSettings = this.Configuration.GetSection("AppSettings:Authentication").Get<AuthenticationSettings>();
            var key = Encoding.ASCII.GetBytes(authSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            // Configure Cors
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
            });

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

        private static HttpClientHandler ConfigureGarminClientMessageHandler()
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
