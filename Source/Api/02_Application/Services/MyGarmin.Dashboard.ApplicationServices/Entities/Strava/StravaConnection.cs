using System;

namespace MyGarmin.Dashboard.ApplicationServices.Entities.Strava
{
    public class StravaConnection
    {
        public string ClientId { get; set; }

        public string Secret { get; set; }

        public string Token { get; set; }

        public long WebhookSubscriptionId { get; set; }

        public string RefreshToken { get; set; }

        public DateTime TokenExpirationDate { get; set; }

        public bool IsDataLoaded { get; set; }

        public DateTime? LastUpdate { get; set; }
    }
}
