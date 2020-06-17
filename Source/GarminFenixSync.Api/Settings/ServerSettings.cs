using System;

namespace GarminFenixSync.Api.Settings
{
    public class ServerSettings
    {
        public string Host { get; set; }

        public int Port { get; set; }

        public TimeSpan ShutdownTimeout { get; set; }
    }
}
