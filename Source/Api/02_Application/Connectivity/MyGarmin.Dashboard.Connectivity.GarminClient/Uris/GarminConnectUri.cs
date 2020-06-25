using System;

namespace MyGarmin.Dashboard.Connectivity.GarminClient.Uris
{
    public static class GarminConnectUri
    {
        private const string Hostname = "connect.garmin.com";
        public static readonly Uri Signin = new Uri(new UriBuilder(Uri.UriSchemeHttps, Hostname).Uri, "sigin/");
        public static readonly Uri Css = new Uri(new UriBuilder(Uri.UriSchemeHttps, Hostname).Uri, "gauth-custom-v1.2-min.css");
        public static readonly Uri Modern = new Uri(new UriBuilder(Uri.UriSchemeHttps, Hostname).Uri, "modern/");
    }
}
