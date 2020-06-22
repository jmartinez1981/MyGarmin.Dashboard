using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Debugging;
using System;
using System.IO;

namespace MyGarmin.Dashboard.Common.Logging
{
    /// <summary>
    /// Microsoft dependency injections for serilog.
    /// </summary>
    public static class SerilogExtensions
    {
        private const string LogFileDirectory = "Log";

        /// <summary>
        /// Configures the format for messages tracing.
        /// </summary>
        /// <param name="loggerConfiguration">The logger configuration.</param>
        /// <param name="configuration">The host configuration</param>
        public static void ConfigureSerilog(this LoggerConfiguration loggerConfiguration, IConfiguration configuration)
        {
            if (!Directory.Exists(LogFileDirectory))
            {
                _ = Directory.CreateDirectory(LogFileDirectory);
            }

            loggerConfiguration?.ReadFrom
                .Configuration(configuration);

            SelfLog.Enable(async msg =>
            {
                await Console.Error.WriteLineAsync(msg).ConfigureAwait(false);
                using var file = new StreamWriter(".\\Log\\GarminFenixSync.Serilog.SelfLog.log", true);
                await file.WriteLineAsync(msg).ConfigureAwait(false);
            });
        }
    }
}
