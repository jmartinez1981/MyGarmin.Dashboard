using System;

namespace MyGarmin.Dashboard.Connectivity.StravaClient.Uris
{
    public static class AthleteUri
    {
        public static Uri AuthenticatedAthlete(this Uri uri) => new Uri(uri, "athlete");

        public static Uri Equipment(this Uri uri, string id) {

            return new Uri(uri, $"gear/{id}");
        }

        public static Uri Activity(this Uri uri, long activityId) => new Uri(uri, $"activities/{activityId}");

        public static Uri Activities(this Uri uri, string athleteId, int pageNumber, int dataPerPage)
        {
            return
                new Uri(uri, $"athletes/{athleteId}/activities")
                    .AddParameter("page", pageNumber.ToString(System.Globalization.CultureInfo.InvariantCulture))
                    .AddParameter("per_page", dataPerPage.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }
    }
}
