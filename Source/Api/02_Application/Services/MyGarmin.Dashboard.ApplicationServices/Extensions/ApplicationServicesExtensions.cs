using Microsoft.Extensions.DependencyInjection;
using MyGarmin.Dashboard.ApplicationServices.Interfaces;

namespace MyGarmin.Dashboard.ApplicationServices.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IStravaConnectionsService, StravaConnectionsService>();
            services.AddScoped<IStravaImportService, StravaImportService>();
            services.AddScoped<IStravaSubscriptionsService, StravaSubscriptionsService>();
            services.AddScoped<IStravaActivitiesService, StravaActivitiesService>();
            services.AddScoped<IStravaAthletesService, StravaAthletesService>();
            services.AddScoped<IGarminConnectionService, GarminConnectionsService>();
            services.AddScoped<IConnectionsService, ConnectionsService>();

            return services;
        }
    }
}
