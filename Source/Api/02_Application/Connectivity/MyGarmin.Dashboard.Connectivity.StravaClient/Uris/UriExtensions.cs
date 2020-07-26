using System;
using System.Web;

namespace MyGarmin.Dashboard.Connectivity.StravaClient.Uris
{
    internal static class UriExtensions
    {
        public static Uri AddParameter(this Uri uri, string paramName, string paramValue, bool withEncoding = true)
        {
            var uriBuilder = new UriBuilder(uri);

            if (withEncoding)
            {
                var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query[paramName] = paramValue;
                uriBuilder.Query = query.ToString();
            }
            else
            {
                uriBuilder.Query += $"&{paramName}={paramValue}";
            }
            
            return uriBuilder.Uri;
        }
    }
}
