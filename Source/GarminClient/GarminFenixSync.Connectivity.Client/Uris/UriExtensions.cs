using System;
using System.Web;

namespace MyGarmin.Connectivity.Client.Uris
{
    public static class UriExtensions
    {
        public static Uri AddParameter(this Uri uri, string paramName, string paramValue)
        {
            var uriBuilder = new UriBuilder(uri);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query[paramName] = paramValue;
            uriBuilder.Query = query.ToString();

            return uriBuilder.Uri;
        }

        public static string ToStringWithoutLastSlash(this Uri uri)
        {
            return uri.ToString().EndsWith("/", StringComparison.InvariantCultureIgnoreCase) ? uri.ToString().Substring(0, uri.ToString().Length - 1) : uri.ToString();
        }
    }
}
