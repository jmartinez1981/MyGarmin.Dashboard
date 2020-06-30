using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyGarmin.Dashboard.Common.Settings;
using System;

namespace MyGarmin.Dashboard.Api.Hosting.Extensions
{
    public static class SettingExtensions
    {
        public static IServiceCollection ConfigureSettings(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            services.Configure<ServerSettings>(configuration.GetSection("AppSettings:Server"));
            services.Configure<GarminConnectionSettings>(configuration.GetSection("AppSettings:GarminConnection"));
            services.Configure<AuthenticationSettings>(configuration.GetSection("AppSettings:Authentication"));

            services.Configure<HostOptions>(
                    opts => opts.ShutdownTimeout = configuration.GetValue<TimeSpan>("AppSettings:Server:ShutdownTimeout"));

            return services;
        }
    }
}
