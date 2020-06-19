using System;

namespace GarminFenixSync.Connectivity.Client.Uris
{
    internal static class Garmin
    {
        private const string BaseAddress = "www.garmin.com";
        private static readonly Uri Host = new Uri($"https://{BaseAddress}");
        public static readonly Uri PrivacyStatementUri = new Uri(Host, "en-US/privacy/connect/");
    }
}
