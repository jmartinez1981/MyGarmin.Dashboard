using System;

namespace MyGarmin.Dashboard.Api.Models
{
    public class StravaConnectionModel
    {
        public string Id { get; set; }

        public string Token { get; set; }

        public string RefreshToken { get; set; }

        public bool IsDataLoaded { get; set; }

        public DateTime? LastUpdate { get; set; }
    }
}
