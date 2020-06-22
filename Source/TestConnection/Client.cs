using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TestConnection
{
    public class Client
    {
        private const string LOCALE = "en_US";
        private const string USER_AGENT = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:64.0) Gecko/20100101 Firefox/64.0";
        private const string CONNECT_DNS = "connect.garmin.com";
        private const string CONNECT_URL = "https://" + CONNECT_DNS;
        private const string CONNECT_URL_MODERN = CONNECT_URL + "/modern/";
        private const string CONNECT_URL_SIGNIN = CONNECT_URL + "/signin/";
        private const string SSO_DNS = "sso.garmin.com";
        private const string SSO_URL = "https://" + SSO_DNS;
        private const string SSO_URL_SSO = SSO_URL + "/sso";
        private const string SSO_URL_SSO_SIGNIN = SSO_URL_SSO + "/signin";
        private const string CONNECT_URL_PROFILE = CONNECT_URL_MODERN + "proxy/userprofile-service/socialProfile/";
        private const string CONNECT_MODERN_HOSTNAME = "https://connect.garmin.com/modern/auth/hostname";
        private const string CSS_URL = CONNECT_URL + "/gauth-custom-v1.2-min.css";
        private const string PRIVACY_STATEMENT_URL = "https://www.garmin.com/en-US/privacy/connect/";
        private const string URL_UPLOAD = CONNECT_URL + "/modern/proxy/upload-service/upload";
        private const string URL_ACTIVITY_BASE = CONNECT_URL + "/modern/proxy/activity-service/activity";

        private const string UrlActivityTypes =
            "https://connect.garmin.com/modern/proxy/activity-service/activity/activityTypes";

        private const string UrlEventTypes =
            "https://connect.garmin.com/modern/proxy/activity-service/activity/eventTypes";

        private const string UrlActivitiesBase =
            "https://connect.garmin.com/modern/proxy/activitylist-service/activities/search/activities";

        private const string UrlActivityDownloadFile =
            " https://connect.garmin.com/modern/proxy/download-service/export/{0}/activity/{1}";

        private const string UrlActivityDownloadDefaultFile =
            " https://connect.garmin.com/modern/proxy/download-service/files/activity/{0}";

        private static CookieContainer cookieContainer;
        private static HttpClientHandler clientHandler;
        private HttpClient httpClient;

        private static readonly Tuple<string, string> BaseHeader = new Tuple<string, string>("NK", "NT");

        private static readonly Dictionary<string, string> QueryParams = new Dictionary<string, string>
        {
            {"clientId", "GarminConnect"},
            {"connectLegalTerms", "true"},
            {"consumeServiceTicket", "false"},
            {"createAccountShown", "true"},
            {"cssUrl", CSS_URL},
            {"displayNameShown", "false"},
            {"embedWidget", "false"},
            // ReSharper disable once StringLiteralTypo
            {"gauthHost", SSO_URL_SSO},
            {"generateExtraServiceTicket", "true"},
            {"generateTwoExtraServiceTickets", "false"},
            {"generateNoServiceTicket", "false"},
            {"globalOptInChecked", "false"},
            {"globalOptInShown", "true"},
            // ReSharper disable once StringLiteralTypo
            {"id", "gauth-widget"},
            {"initialFocus", "true"},
            {"locale", LOCALE},
            {"locationPromptShon", "true"},
            {"mobile", "false"},
            {"openCreateAccount", "false"},
            {"privacyStatementUrl", PRIVACY_STATEMENT_URL},
            {"redirectAfterAccountCreationUrl", CONNECT_URL_MODERN},
            {"redirectAfterAccountLoginUrl", CONNECT_URL_MODERN},
            {"rememberMeChecked", "false"},
            {"rememberMeShown", "true"},
            {"service", CONNECT_URL_MODERN},
            {"showTermsOfUse", "false"},
            {"showPrivacyPolicy", "false"},
            {"showConnectLegalAge", "false"},
            {"showPassword", "true"},
            {"source", CONNECT_URL_SIGNIN},
            // {"usernameShown", "false"},
            {"useCustomHeader", "false"},
            {"webhost", CONNECT_URL_MODERN}
        };

        public Client()
        {
        }

        public async Task Authenticate()
        {
            cookieContainer = new CookieContainer();
            clientHandler =
                new HttpClientHandler
                {
                    AllowAutoRedirect = true,
                    UseCookies = true,
                    CookieContainer = cookieContainer
                };

            this.httpClient = new HttpClient(clientHandler);

            this.httpClient.DefaultRequestHeaders.Add("user-agent", USER_AGENT);
            var data = await this.httpClient.GetStringAsync(CONNECT_MODERN_HOSTNAME);

            var ssoHostname = JObject.Parse(data)["host"] == null
                ? throw new Exception("SSO hostname is missing")
                : JObject.Parse(data)["host"].ToString();

            var queryParams = string.Join("&", QueryParams.Select(e => $"{e.Key}={WebUtility.UrlEncode(e.Value)}"));

            var url = $"{SSO_URL_SSO_SIGNIN}?{queryParams}";
            var res = await this.httpClient.GetAsync(url);
            ValidateResponseMessage(res, "No login form.");

            data = await res.Content.ReadAsStringAsync();
            var csrfToken = "";
            try
            {
                GetValueByPattern(data, @"input type=\""hidden\"" name=\""_csrf\"" value=\""(\w+)\"" \/>", 2, 1);
            }
            catch (Exception e)
            {
                throw e;
            }

            this.httpClient.DefaultRequestHeaders.Add("origin", SSO_URL);
            this.httpClient.DefaultRequestHeaders.Add("referer", url);

            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("embed", "false"),
                new KeyValuePair<string, string>("username", "jmf.stk@gmail.com"),
                new KeyValuePair<string, string>("password", "Jose.martinez1981"),
                new KeyValuePair<string, string>("_csrf", csrfToken)
            });

            res = await this.httpClient.PostAsync(url, formContent);
            data = await res.Content.ReadAsStringAsync();
            ValidateResponseMessage(res, $"Bad response {res.StatusCode}, expected {HttpStatusCode.OK}");
            ValidateCookiePresence(cookieContainer, "GARMIN-SSO-GUID");

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
            ValidateResponseMessage(res, "Login check failed.");
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
            var cookies = container.GetCookies(new Uri(CONNECT_URL_MODERN)).Cast<Cookie>().ToList();
            if (!cookies.Any(e => string.Equals(cookieName, e.Name, StringComparison.InvariantCultureIgnoreCase)))
            {
                throw new Exception($"Missing cookie {cookieName}");
            }
        }

        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
        private static void ValidateResponseMessage(HttpResponseMessage responseMessage, string errorMessage)
        {
            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new Exception(errorMessage);
            }
        }

        private void ValidateModernTicketUrlResponseMessage(HttpResponseMessage responseMessage, string error)
        {
            if (!responseMessage.IsSuccessStatusCode && !responseMessage.StatusCode.Equals(HttpStatusCode.MovedPermanently))
            {
                throw new Exception(error);
            }
        }
                
        /// <summary>
        /// Gets the unix timestamp.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        private static int GetUnixTimestamp(DateTime date)
        {
            return (int)date.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }

        /// <summary>
        /// Creates the activities URL.
        /// </summary>
        /// <param name="limit">The limit.</param>
        /// <param name="start">The start.</param>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        private static string CreateActivitiesUrl(int limit, int start, DateTime date)
        {
            return $"{UrlActivitiesBase}?limit={limit}&start={start}&_={GetUnixTimestamp(date)}";
        }
                
        private static T DeserializeData<T>(string data) where T : class
        {
            return typeof(T) == typeof(string) ? data as T : JsonConvert.DeserializeObject<T>(data);
        }
    }
}
