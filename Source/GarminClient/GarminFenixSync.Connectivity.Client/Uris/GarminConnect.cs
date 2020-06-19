using System;

namespace GarminFenixSync.Connectivity.Client.Uris
{
    internal static class GarminConnect
    {
        private const string BaseAddress = "connect.garmin.com";
        private static readonly Uri Host = new Uri($"https://{BaseAddress}");
        
        internal static readonly Uri ModernUri = new Uri(Host, "modern/");
        internal static readonly Uri SiginUri = new Uri(Host, "signin/");
        internal static readonly Uri CssUri = new Uri(Host, "gauth-custom-v1.2-min.css");
        internal static readonly Uri ProfileUri = new Uri(ModernUri, "proxy/userprofile-service/socialProfile/");
        internal static readonly Uri AuthHostnameUri = new Uri(ModernUri, "auth/hostname");
        internal static readonly Uri UploadUri = new Uri(ModernUri, "proxy/upload-service/upload");
        internal static readonly Uri ActivityUri = new Uri(ModernUri, "proxy/activity-service/activity");
    }
}
