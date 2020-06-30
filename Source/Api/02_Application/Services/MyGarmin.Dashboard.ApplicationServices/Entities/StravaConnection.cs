using System;

namespace MyGarmin.Dashboard.ApplicationServices.Entities
{
    public class StravaConnection
    {
        public string ClientId { get; set; }

        public string Token { get; set; }

        public string RefreshToken { get; set; }

        public bool IsDataLoaded { get; set; }

        public DateTime? LastUpdate { get; set; }
    }
}
