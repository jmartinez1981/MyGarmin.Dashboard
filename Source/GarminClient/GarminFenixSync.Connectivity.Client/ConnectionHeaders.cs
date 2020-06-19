using GarminFenixSync.Connectivity.Client.Uris;
using System.Collections.Generic;

namespace GarminFenixSync.Connectivity.Client
{
    internal static class ConnectionHeaders
    {
        internal const string UserAgentKey = "USER_AGENT";
        internal const string OriginKey = "origin";
        internal const string RefererKey = "referer";

        private static readonly Dictionary<string, string> headers = new Dictionary<string, string>
        {
            {UserAgentKey, "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:64.0) Gecko/20100101 Firefox/64.0"},
            {OriginKey, GarminSso.SsoUri.ToString()},
        };

        public static string GetHeaderValue(string headerKey) => headers[headerKey];
    }
}
