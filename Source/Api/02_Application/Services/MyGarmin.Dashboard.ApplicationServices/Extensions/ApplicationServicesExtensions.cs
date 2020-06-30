using Microsoft.Extensions.DependencyInjection;

namespace MyGarmin.Dashboard.ApplicationServices.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IStravaConnectionService, StravaConnectionService>();
            services.AddScoped<IGarminConnectionService, GarminConnectionService>();
            services.AddScoped<IConnectionService, ConnectionService>();

            return services;
        }
    }
}
