using System;
using System.Web;

namespace TestConnection
{
    public static class UriExtensions
    {
        public static Uri WebHostUri = new Uri("https://connect.garmin.com/");
        public static Uri SigninUri = new Uri("https://connect.garmin.com/signin/");
        public static Uri ModernUri = new Uri("https://connect.garmin.com/modern/");
        public static Uri AuthUri = new Uri("https://connect.garmin.com/modern/auth/hostname/");
        public static Uri SsoBaseUri = new Uri("https://sso.garmin.com");
        public static Uri SsoUri = new Uri("https://sso.garmin.com/sso");
        public static Uri SsoSigninUri = new Uri("https://sso.garmin.com/sso/signin");
        public static Uri CssUri = new Uri("https://connect.garmin.com/gauth-custom-v1.2-min.css");
        public static Uri PrivacityStatementUri = new Uri("https://www.garmin.com/en-US/privacy/connect/");

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
            return uri.ToString().EndsWith("/") ? uri.ToString().Substring(0, uri.ToString().Length - 1) : uri.ToString();
        }
    }
}
