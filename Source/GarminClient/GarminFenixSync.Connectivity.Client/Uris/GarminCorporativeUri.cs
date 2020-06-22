using System;

namespace MyGarmin.Connectivity.Client.Uris
{
    public static class GarminCorporativeUri
    {
        private const string Hostname = "www.garmin.com";
        public static readonly Uri Privacy = new Uri(new UriBuilder(Uri.UriSchemeHttps, Hostname).Uri, "en-US/privacy/connect/");
    }
}
