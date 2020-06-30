using System;

namespace MyGarmin.Dashboard.Api.Models
{
    public class ConnectionModel
    {
        public string ConnectionType { get; set; }

        public string Id { get; set; }

        public string Token { get; set; }

        public string RefreshToken { get; set; }

        public string Password { get; set; }

        public bool IsDataLoaded { get; set; }

        public DateTime? LastUpdate { get; set; }
    }
}
