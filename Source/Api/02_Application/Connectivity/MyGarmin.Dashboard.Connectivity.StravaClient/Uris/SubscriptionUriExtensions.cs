using System;

namespace MyGarmin.Dashboard.Connectivity.StravaClient.Uris
{
    public static class SubscriptionUriExtensions
    {
        public static Uri SubscriptionCreation(this Uri uri, string clientId, string secret, Uri callbackUri, string verifyToken) 
        {
            return new Uri(uri, "push_subscriptions")
                .AddParameter("client_id", clientId)
                .AddParameter("client_secret", secret)
                .AddParameter("callback_url", callbackUri?.ToString())
                .AddParameter("verify_token", verifyToken);
        }
    }
}
