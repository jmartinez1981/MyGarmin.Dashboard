using Microsoft.Extensions.Options;
using MyGarmin.Dashboard.ApplicationServices.Interfaces;
using MyGarmin.Dashboard.Common.Settings;
using MyGarmin.Dashboard.Connectivity.StravaClient;
using System;
using System.Threading.Tasks;

namespace MyGarmin.Dashboard.ApplicationServices
{
    internal class StravaSubscriptionsService : IStravaSubscriptionsService
    {
        private const string SubscriptionMode = "subscribe";

        private readonly IStravaSubscriptionClient stravaSubscriptionClient;
        private readonly IOptions<StravaConnectionSettings> settings;

        public StravaSubscriptionsService(
            IStravaSubscriptionClient stravaSubscriptionClient,
            IOptions<StravaConnectionSettings> settings)
        {
            this.stravaSubscriptionClient = stravaSubscriptionClient;
            this.settings = settings;
        }

        public async Task<long> Subscribe(string clientId, string clientSecret)
        {
            var callbackUri = new Uri(this.settings.Value.SubscriptionCallback);
            var verifyToken = this.settings.Value.SubscriptionVerifyToken;
            return await this.stravaSubscriptionClient.SubscriptionCreationRequest(clientId, clientSecret, callbackUri, verifyToken);
        }

        public bool IsSubscriptionCallbackValid(string verifyToken, string mode)
        {
            return verifyToken == this.settings.Value.SubscriptionVerifyToken && mode == SubscriptionMode;
        }
    }
}
