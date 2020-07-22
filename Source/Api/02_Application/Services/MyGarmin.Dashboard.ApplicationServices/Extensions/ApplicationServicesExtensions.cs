using Microsoft.Extensions.DependencyInjection;
using MyGarmin.Dashboard.ApplicationServices.Interfaces;

namespace MyGarmin.Dashboard.ApplicationServices.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IStravaConnectionService, StravaConnectionService>();
            services.AddScoped<IStravaImportService, StravaImportService>();
            services.AddScoped<IGarminConnectionService, GarminConnectionService>();
            services.AddScoped<IConnectionService, ConnectionService>();

            return services;
        }
    }
}
