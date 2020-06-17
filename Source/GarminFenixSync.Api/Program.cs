using GarminFenixSync.Api.Hosting;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace GarminFenixSync.Api
{
    internal static class Program
    {
        private const string ConsoleTitle = "GarminFenixSync.Api";

        internal static async Task Main(string[] args)
        {
            var hostConfigurator = new WebHostConfigurator();
            var hostBuilder = hostConfigurator.ConfigureHost(new HostBuilder());

            if (Debugger.IsAttached || args.Contains("--console"))
            {
                await WebHostConfigurator.RunAsConsoleAsync(hostBuilder, ConsoleTitle).ConfigureAwait(false);
            }
            else
            {
                await WebHostConfigurator.RunAsServiceAsync(hostBuilder).ConfigureAwait(false);
            }
        }
    }
}
