using System;
using System.Web;

namespace MyGarmin.Dashboard.Connectivity.StravaClient.Uris
{
    internal static class UriExtensions
    {
        public static Uri AddParameter(this Uri uri, string paramName, string paramValue)
        {
            var uriBuilder = new UriBuilder(uri);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query[paramName] = paramValue;
            uriBuilder.Query = query.ToString();

            return uriBuilder.Uri;
        }
    }
}
