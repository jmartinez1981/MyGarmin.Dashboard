using GarminFenixSync.Connectivity.Client.Uris;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
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

        private async Task Authenticate(string username, string password)
        {
            this.httpClient.DefaultRequestHeaders.Add(ConnectionHeaders.UserAgentKey, ConnectionHeaders.GetHeaderValue(ConnectionHeaders.UserAgentKey));
            var data = await this.httpClient.GetStringAsync(GarminConnect.AuthHostnameUri).ConfigureAwait(false);

            var ssoJson = System.Text.Json.JsonDocument.Parse(data);
            if (!ssoJson.RootElement.TryGetProperty("host", out var ssoHostname))
            {
                throw new Exception("SSO hostname is missing");
            }

            var siginUrl = QueryStringExtensions.GetQueryString(GarminSso.SigninUri);
            var res = await this.httpClient.GetAsync(siginUrl).ConfigureAwait(false);
            res.EnsureSuccessStatusCode();

            data = await res.Content.ReadAsStringAsync().ConfigureAwait(false);
            var csrfToken = string.Empty;

            GetValueByPattern(data, @"input type=\""hidden\"" name=\""_csrf\"" value=\""(\w+)\"" \/>", 2, 1);

            this.httpClient.DefaultRequestHeaders.Add(ConnectionHeaders.OriginKey, ConnectionHeaders.GetHeaderValue(ConnectionHeaders.OriginKey));
            this.httpClient.DefaultRequestHeaders.Add(ConnectionHeaders.RefererKey, siginUrl.ToString());

            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("embed", "false"),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password),
                new KeyValuePair<string, string>("_csrf", csrfToken)
            });

            res = await this.httpClient.PostAsync(siginUrl.ToString(), formContent);
            res.EnsureSuccessStatusCode();
            data = await res.Content.ReadAsStringAsync();

            //ValidateCookiePresence(cookieContainer, "GARMIN-SSO-GUID");

            var ticket = GetValueByPattern(data, @"var response_url(\s+)= (\""|\').*?ticket=([\w\-]+)(\""|\')", 5, 3);

            // Second auth step
            // Needs a service ticket from previous response
            this.httpClient.DefaultRequestHeaders.Remove("origin");
            url = $"{CONNECT_URL_MODERN}?ticket={WebUtility.UrlEncode(ticket)}";
            res = await this.httpClient.GetAsync(url);

            ValidateModernTicketUrlResponseMessage(res, $"Second auth step failed to produce success or expected 302: {res.StatusCode}.");

            // Check session cookie
            ValidateCookiePresence(cookieContainer, "SESSIONID");

            // Check login
            res = await this.httpClient.GetAsync(CONNECT_URL_PROFILE);
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

        /// <summary>
        /// Validates the cookie presence.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="cookieName">Name of the cookie.</param>
        /// <exception cref="Exception">Missing cookie {cookieName}</exception>
        private static void ValidateCookiePresence(CookieContainer container, string cookieName)
        {
            var cookies = container.GetCookies(GarminConnect.ModernUri).Cast<Cookie>().ToList();
            if (!cookies.Any(e => string.Equals(cookieName, e.Name, StringComparison.InvariantCultureIgnoreCase)))
            {
                throw new Exception($"Missing cookie {cookieName}");
            }
        }
    }
}
