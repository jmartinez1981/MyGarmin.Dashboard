using GarminFenixSync.Connectivity.Client.Uris;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace GarminFenixSync.Connectivity.Client
{
    internal class Client
    {
        private readonly HttpClient httpClient;
        private readonly ILogger<Client> logger;

        public Client(
            HttpClient httpClient,
            ILogger<Client> logger)
        {
            this.httpClient = httpClient;
            this.logger = logger;
        }

        public void Connect()
        {

        } 

        private async Task Authenticate()
        {
            this.httpClient.DefaultRequestHeaders.Add(ConnectionHeaders.UserAgentKey, ConnectionHeaders.GetHeaderValue(ConnectionHeaders.UserAgentKey));
            var data = await this.httpClient.GetStringAsync(GarminConnect.AuthHostnameUri).ConfigureAwait(false);

            var ssoJson = System.Text.Json.JsonDocument.Parse(data);
            if (!ssoJson.RootElement.TryGetProperty("host", out var ssoHostname))
            {
                throw new Exception("SSO hostname is missing");
            }

            var res = await this.httpClient.GetAsync(QueryStringExtensions.GetQueryString()).ConfigureAwait(false);
            //ValidateResponseMessage(res, "No login form.");
        }
    }
}
