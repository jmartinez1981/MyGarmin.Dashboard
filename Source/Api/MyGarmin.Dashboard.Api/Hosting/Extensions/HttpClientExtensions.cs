using Microsoft.Extensions.DependencyInjection;
using MyGarmin.Connectivity.Client;
using MyGarmin.Dashboard.Api.DelegatingHandlers;
using MyGarmin.Dashboard.ApplicationServices;
using MyGarmin.Dashboard.Connectivity.StravaClient;
using MyGarmin.Dashboard.Connectivity.StravaClient.Uris;
using System.Net;
using System.Net.Http;

namespace MyGarmin.Dashboard.Api.Hosting.Extensions
{
    public static class HttpClientExtensions
    {
        public static IServiceCollection ConfigureGarminClient(this IServiceCollection services)
        {
            services.AddHttpClient<IGarminClient, GarminClient>()
                .ConfigurePrimaryHttpMessageHandler(() => ConfigureGarminClientMessageHandler());

            return services;
        }

        public static IServiceCollection ConfigureStravaClients(this IServiceCollection services)
        {
            ConfigureStravaAuthClient(services);
            ConfigureStravaTokenClient(services);

            return services;
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

        private static void ConfigureStravaAuthClient(IServiceCollection services)
        {
            services.AddScoped<StravaAuthenticationDelegatingHandler>();

            //services.AddHttpClient<IStravaConnectionService, StravaConnectionService>(client =>
            //{
            //    client.BaseAddress = ApiV3Uri.BaseApi;
            //});


            services.AddHttpClient<IStravaAuthClient, StravaAuthClient>(client =>
            {
                client.BaseAddress = ApiV3UriExtensions.BaseApi;
            })
            .AddHttpMessageHandler<StravaAuthenticationDelegatingHandler>();
        }

        private static void ConfigureStravaTokenClient(IServiceCollection services)
        {
            services.AddHttpClient<IStravaTokenClient, StravaTokenClient>(client =>
            {
                client.BaseAddress = AuthUriExtensions.BaseApi;
            });
        }
    }
}
