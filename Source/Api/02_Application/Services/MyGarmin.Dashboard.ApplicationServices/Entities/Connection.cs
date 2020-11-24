using System;

namespace MyGarmin.Dashboard.ApplicationServices.Entities
{
    public class Connection
    {
        public ConnectionType Type { get; set; }

        public string ClientId { get; set; }

        public string Token { get; set; }

        public string RefreshToken { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public bool IsDataLoaded { get; set; }

        public DateTime? LastUpdate { get; set; }
    }
}
