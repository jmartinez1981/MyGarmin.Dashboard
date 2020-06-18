using GarminFenixSync.Common.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace GarminFenixSync.Api.Hosting
{
    [ExcludeFromCodeCoverage]
    public class WebHostConfigurator
    {
        private const string AppSettingsFilename = "appsettings";
        private const string LoggingConfigurationFilename = "loggingConfiguration";

        public static async Task RunAsConsoleAsync(IHostBuilder hostBuilder, string consoleTitle)
        {
            Console.Title = consoleTitle;
            await hostBuilder.RunConsoleAsync().ConfigureAwait(false);
        }

        public static async Task RunAsServiceAsync(IHostBuilder hostBuilder)
        {
            await hostBuilder.UseWindowsService().UseSystemd()
                .Build().RunAsync().ConfigureAwait(false);
        }

        public IHostBuilder ConfigureHost(IHostBuilder hostBuilder)
        {
            ConfigureErrorsOnStartup(hostBuilder);
            this.ConfigureStartup(hostBuilder);
            this.ConfigureServer(hostBuilder);
            this.ConfigureAppConfiguration(hostBuilder);
            ConfigureLogging(hostBuilder);

            return hostBuilder;
        }

        protected virtual void ConfigureAppConfiguration(IHostBuilder hostBuilder)
        {
            hostBuilder?
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    configApp.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "Configuration"));
                    configApp.AddJsonFile($"{AppSettingsFilename}.json", optional: true);
                    //configApp.AddJsonFile($"{AppSettingsFilename}.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true);
                    configApp.AddJsonFile($"{LoggingConfigurationFilename}.json", optional: true);
                    //configApp.AddJsonFile($"{LoggingConfigurationFilename}.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true);
                });
        }

        private static void ConfigureLogging(IHostBuilder hostBuilder)
        {
            hostBuilder
                .UseSerilog((hostingContext, loggerConfiguration) =>
                {
                    loggerConfiguration.ConfigureSerilog(hostingContext.Configuration);
                });
        }

        protected virtual void ConfigureServer(IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureWebHost(webBuilder =>
            {
                webBuilder.UseKestrel((webHostBuilderContext, options) =>
                {
                    var apiHost = webHostBuilderContext.Configuration.GetValue<string>("AppSettings:Server:Host");
                    var apiPort = webHostBuilderContext.Configuration.GetValue<int>("AppSettings:Server:Port");
                    var address = new IPEndPoint(IPAddress.Parse(apiHost), apiPort);
                    options.Listen(address, listenOptions => 
                    {
                        //listenOptions.UseHttps();
                    });
                });
            });
        }

        protected virtual void ConfigureStartup(IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureWebHost(webHostBuilder =>
            {
                webHostBuilder.UseStartup<Startup>();
            });
        }
        
        private static void ConfigureErrorsOnStartup(IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureWebHost(webHostBuilder =>
            {
                webHostBuilder.CaptureStartupErrors(true);
            });
        }
    }
}
