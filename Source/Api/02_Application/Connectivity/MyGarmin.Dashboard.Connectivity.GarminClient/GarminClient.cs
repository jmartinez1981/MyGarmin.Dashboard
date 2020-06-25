using Microsoft.Extensions.Logging;
using MyGarmin.Dashboard.Connectivity.GarminClient.Data;
using MyGarmin.Dashboard.Connectivity.GarminClient.Uris;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyGarmin.Connectivity.Client
{
    public class GarminClient : IGarminClient
    {
        private const string CookieSsoId = "GARMIN-SSO-GUID";
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
            var activities = await DownloadActivities().ConfigureAwait(false);
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
            
            // Check login
            response = await this.httpClient.GetAsync(GarminCorporativeUri.Privacy).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
        }

        private async Task<List<Activity>> DownloadActivities()
        {
            const int limit = 50;
            bool remainingActivities = true;
            int totalActivities = 0;
            var activities = new List<Activity>();

            while(remainingActivities)
            {
                var url = DownloadUri.Download(limit, totalActivities);
                this.httpClient.DefaultRequestHeaders.Add("NK", "NT");
                var response = await this.httpClient.GetAsync(url).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var currentActivities = JsonSerializer.Deserialize<List<Activity>>(content);
                
                if (!currentActivities.Any())
                {
                    remainingActivities = false;
                }
                else
                {
                    totalActivities += currentActivities.Count;
                    activities.AddRange(currentActivities);
                    this.logger.LogInformation($"Activities downloaded: {totalActivities}");
                }
            }

            return activities;
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

            if (!responseCookies.AsEnumerable<string>().Any(x => x.Contains(cookieName)))
            {
                this.logger.LogError($"cookie not exists: {cookieName}");
            }
        }
    }
}
