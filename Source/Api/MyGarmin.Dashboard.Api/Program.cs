using Microsoft.Extensions.Hosting;
using MyGarmin.Dashboard.Api.Hosting;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MyGarmin.Dashboard.Api
{
    internal static class Program
    {
        private const string ConsoleTitle = "MyGarmin.Dashboard.Api";

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
