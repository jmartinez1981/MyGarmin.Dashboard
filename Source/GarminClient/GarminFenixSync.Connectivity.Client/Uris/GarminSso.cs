using System;

namespace GarminFenixSync.Connectivity.Client.Uris
{
    internal static class GarminSso
    {
        private const string BaseAddress = "sso.garmin.com";
        private static readonly Uri Host = new Uri($"https://{BaseAddress}");

        internal static readonly Uri SsoUri = new Uri($"{Host}/sso");
        internal static readonly Uri SigninUri = new Uri(SsoUri, "signin");
    }
}
