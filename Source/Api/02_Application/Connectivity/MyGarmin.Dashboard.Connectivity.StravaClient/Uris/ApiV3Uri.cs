using System;

namespace MyGarmin.Dashboard.Connectivity.StravaClient.Uris
{
    public static class ApiV3Uri
    {
        private const string Hostname = "www.strava.com";

        public static readonly Uri BaseApi = new Uri(new UriBuilder(Uri.UriSchemeHttps, Hostname).Uri, "api/v3/");

    }
}
