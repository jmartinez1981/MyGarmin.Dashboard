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
            ConfigureStravaWebhookClient(services);
            ConfigureStravaTokenClient(services);
            ConfigureStravaSubscriptionClient(services);

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

        private static void ConfigureStravaSubscriptionClient(IServiceCollection services)
        {
            services.AddHttpClient<IStravaSubscriptionClient, StravaSubscriptionClient>(client =>
            {
                client.BaseAddress = ApiV3UriExtensions.BaseApi;
            });
        }

        private static void ConfigureStravaWebhookClient(IServiceCollection services)
        {
            services.AddScoped<StravaWebhookDelegatingHandler>();

            services.AddHttpClient<IStravaWebhookClient, StravaWebhookClient>(client =>
            {
                client.BaseAddress = ApiV3UriExtensions.BaseApi;
            })
            .AddHttpMessageHandler<StravaWebhookDelegatingHandler>();
        }

        private static void ConfigureStravaAuthClient(IServiceCollection services)
        {
            services.AddScoped<StravaAuthenticationDelegatingHandler>();

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
