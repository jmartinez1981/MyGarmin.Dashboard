using System;
using System.IO;
using GarminConnectClient.Lib;
using GarminConnectClient.Lib.Dto;
using GarminConnectClient.Lib.Enum;
using GarminConnectClient.Lib.Services;
using Microsoft.Extensions.Configuration;

namespace GarminFenixSync.Api
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = SetupConfiguration();
            var client = new Client(configuration, new ConsoleLogger<Client>());
            client.Authenticate().Wait();
            var eventTypes = client.LoadEventTypes().Result;
            var activityTypes = client.LoadActivityTypes().Result;
            
            var downloader = new Downloader(configuration, client, null, new ConsoleLogger<Client>());

            var result = downloader.DownloadLastUserActivities().GetAwaiter().GetResult();
        }
        

        private static GarminConnectClient.Lib.IConfiguration SetupConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true);

            var configuration = builder.Build();

            return new Configuration(configuration);
        }
    }
}
