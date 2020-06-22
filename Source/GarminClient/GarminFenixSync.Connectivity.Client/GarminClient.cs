using Microsoft.Extensions.Logging;
using MyGarmin.Connectivity.Client.Uris;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyGarmin.Connectivity.Client
{
    public class GarminClient : IGarminClient
    {
        private const string CookieSsoId = "GARMIN-SSO-GUID";
        private const string CookieSessionId = "SESSIONID";

        private readonly HttpClient httpClient;
        private readonly ILogger<GarminClient> logger;

        public GarminClient(
            HttpClient httpClient,
            ILogger<GarminClient> logger)
        {
            this.httpClient = httpClient;
            this.logger = logger;
        }

        public async Task Connect()
        {
            var ticket = await AuthFirstStep().ConfigureAwait(false);
            await AuthSecondStep(ticket).ConfigureAwait(false);
            await DownloadActivities().ConfigureAwait(false);
        }

        private async Task<string> AuthFirstStep()
        {
            // 1. Post login data
            var csrfToken = string.Empty;
            var arrayFormValues = new[]
            {
                        new KeyValuePair<string, string>("embed", "false"),
                        new KeyValuePair<string, string>("username", "jmf.stk@gmail.com"),
                        new KeyValuePair<string, string>("password", "Jose.martinez1981"),
                        new KeyValuePair<string, string>("_csrf", csrfToken)
            };

            using (var formContent = new FormUrlEncodedContent(arrayFormValues))
            {
                var signinUri = SigninConnection.GetSsoSigninUri();


                this.httpClient.DefaultRequestHeaders.Add("origin", GarminSSoUri.SsoBase.ToStringWithoutLastSlash());
                this.httpClient.DefaultRequestHeaders.Add("referer", signinUri.ToString());

                var response = await this.httpClient.PostAsync(signinUri, formContent).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                // Check session cookie
                CheckCookie(response, CookieSsoId);
                                
                return GetValueByPattern(result, @"var response_url(\s+)= (\""|\').*?ticket=([\w\-]+)(\""|\')", 5, 3);
            }
        }

        private async Task AuthSecondStep(string ticket)
        {
            // Needs a service ticket from previous response
            this.httpClient.DefaultRequestHeaders.Remove("origin");

            var authUri = SigninConnection.GetAuthUri(ticket);
            var response = await this.httpClient.GetAsync(authUri).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            // Check session cookie
            //CheckCookie(response, CookieSessionId);
            
            // Check login
            response = await this.httpClient.GetAsync(GarminCorporativeUri.Privacy).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
        }

        private async Task DownloadActivities()
        {
            const int limit = 50;
            var from = DateTime.UtcNow;

            var url = DownloadUri.Download(limit, 0, from);
            this.httpClient.DefaultRequestHeaders.Add("NK", "NT");
            var response = await this.httpClient.GetAsync(url).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the value by pattern.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="pattern">The pattern.</param>
        /// <param name="expectedCountOfGroups">The expected count of groups.</param>
        /// <param name="groupPosition">The group position.</param>
        /// <returns>Value of particular match group.</returns>
        /// <exception cref="Exception">Could not match expected pattern {pattern}</exception>
        private static string GetValueByPattern(string data, string pattern, int expectedCountOfGroups, int groupPosition)
        {
            var regex = new Regex(pattern);
            var match = regex.Match(data);
            if (!match.Success || match.Groups.Count != expectedCountOfGroups)
            {
                throw new Exception($"Could not match expected pattern {pattern}.");
            }
            return match.Groups[groupPosition].Value;
        }

        private void CheckCookie(HttpResponseMessage response, string cookieName)
        {
            response.Headers.TryGetValues("set-cookie", out var responseCookies);

            if (!responseCookies.AsEnumerable<string>().Any(x => x == cookieName))
            {
                this.logger.LogError($"cookie not exists: {cookieName}");
            }
        }
    }
}
