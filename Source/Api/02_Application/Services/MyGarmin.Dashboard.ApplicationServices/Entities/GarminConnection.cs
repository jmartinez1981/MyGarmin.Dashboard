using System;

namespace MyGarmin.Dashboard.ApplicationServices.Entities
{
    public class GarminConnection
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public bool IsDataLoaded { get; set; }

        public DateTime? LastUpdate { get; set; }
    }
}
