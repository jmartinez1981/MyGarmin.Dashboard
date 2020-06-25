using System;

namespace MyGarmin.Dashboard.Connectivity.GarminClient.Uris
{
    public static class GarminSSoUri
    {
        private const string Hostname = "sso.garmin.com";
        public static readonly Uri SsoBase = new Uri(new UriBuilder(Uri.UriSchemeHttps, Hostname).Uri, string.Empty);
        public static readonly Uri Sso = new Uri(new UriBuilder(Uri.UriSchemeHttps, Hostname).Uri, "sso");
        public static readonly Uri Signin = new Uri(new UriBuilder(Uri.UriSchemeHttps, Hostname).Uri, "sso/signin");
    }
}
