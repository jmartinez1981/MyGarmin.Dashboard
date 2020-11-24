using System;

namespace MyGarmin.Dashboard.Connectivity.StravaClient.Uris
{
    public static class ExchangeTokenUriExtensions
    {
        public static Uri ExchangeAuthToken(this Uri uri, string clientId, string secret, string code) 
        {
            return new Uri(uri, "token")
                .AddParameter("client_id", clientId)
                .AddParameter("client_secret", secret)
                .AddParameter("code", code)
                .AddParameter("grant_type", "authorization_code");
        }

        public static Uri RefreshAuthToken(this Uri uri, string clientId, string secret, string refreshToken)
        {
            return new Uri(uri, "token")
                .AddParameter("client_id", clientId)
                .AddParameter("client_secret", secret)
                .AddParameter("grant_type", "refresh_token")
                .AddParameter("refresh_token", refreshToken);
        }

    }
}
