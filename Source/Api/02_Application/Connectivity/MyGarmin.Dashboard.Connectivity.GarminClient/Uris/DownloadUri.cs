using System;

namespace MyGarmin.Dashboard.Connectivity.GarminClient.Uris
{
    public static class DownloadUri
    {
        private const string Hostname = "connect.garmin.com";
        private const string LimitParameterKey = "limit";
        private const string StartParameterKey = "start";
        private const string DateParameterKey = "_";

        private static readonly Uri ActivitiesUri = new Uri(new UriBuilder(Uri.UriSchemeHttps, Hostname).Uri, "modern/proxy/activitylist-service/activities/search/activities");

        public static Uri Download(int limit, int start, DateTime date)
        {
            var uri = ActivitiesUri
                .AddParameter(LimitParameterKey, limit.ToString(System.Globalization.CultureInfo.InvariantCulture))
                .AddParameter(StartParameterKey, start.ToString(System.Globalization.CultureInfo.InvariantCulture));

            if (date != default)
            {
                uri.AddParameter(DateParameterKey, GetUnixTimestamp(date).ToString(System.Globalization.CultureInfo.InvariantCulture));
            }

            return uri;
        }

        public static Uri Download(int limit, int start)
        {
            return Download(limit, start, default);
        }

        private static int GetUnixTimestamp(DateTime date)
        {
            return (int)date.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }
    }
}
