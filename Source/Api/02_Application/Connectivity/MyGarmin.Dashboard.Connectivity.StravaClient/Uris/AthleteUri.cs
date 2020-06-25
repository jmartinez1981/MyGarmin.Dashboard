using System;

namespace MyGarmin.Dashboard.Connectivity.StravaClient.Uris
{
    public static class AthleteUri
    {
        public static Uri AuthenticatedAthlete(this Uri uri) => new Uri(uri, "athlete");
    }
}
