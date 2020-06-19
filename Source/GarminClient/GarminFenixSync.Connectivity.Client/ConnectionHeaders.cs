using System.Collections.Generic;

namespace GarminFenixSync.Connectivity.Client
{
    internal static class ConnectionHeaders
    {
        internal const string LocalKey = "LOCALE";
        internal const string UserAgentKey = "USER_AGENT";
        
        private static readonly Dictionary<string, string> headers = new Dictionary<string, string>
        {
            {LocalKey, "en_US"},
            {UserAgentKey, "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:64.0) Gecko/20100101 Firefox/64.0"},
        };

        public static string GetHeaderValue(string headerKey) => headers[headerKey];
    }
}
