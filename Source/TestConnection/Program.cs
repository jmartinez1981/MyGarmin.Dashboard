using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TestConnection
{
    class Program
    {
        static void Main(string[] args)
        {
            //var clientGarmin = new Client();
            //clientGarmin.Authenticate().GetAwaiter().GetResult();

            using (var clientHandler = new HttpClientHandler())
            {
                var cookieContainer = new CookieContainer();
                clientHandler.AllowAutoRedirect = true;
                clientHandler.UseCookies = true;
                clientHandler.CookieContainer = cookieContainer;

                using (var client = new HttpClient(clientHandler))
                {
                    client.DefaultRequestHeaders.Add("user-agent", @"Mozilla/5.0 (Windows NT 6.1; WOW64; rv:64.0) Gecko/20100101 Firefox/64.0");

                    //var response = client.GetAsync(UriExtensions.AuthUri).GetAwaiter().GetResult();
                    //var content = GetResponseContent(response).GetAwaiter().GetResult();

                    //client.DefaultRequestHeaders.Add("origin", UriExtensions.SsoBaseUri.ToString());

                    // 1. Get a login page
                    //var signinUri = SigninConnection.GetSsoSigninUri();
                    //response = client.GetAsync(signinUri).GetAwaiter().GetResult();
                    //content = GetResponseContent(response).GetAwaiter().GetResult();
                    //GetValueByPattern(content, @"input type=\""hidden\"" name=\""_csrf\"" value=\""(\w+)\"" \/>", 2, 1);

                    // 2. Post login data
                    var signinUri = SigninConnection.GetSsoSigninUri();
                    var csrfToken = string.Empty;
                    var formContent = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("embed", "false"),
                        new KeyValuePair<string, string>("username", "jmf.stk@gmail.com"),
                        new KeyValuePair<string, string>("password", "Jose.martinez1981"),
                        new KeyValuePair<string, string>("_csrf", csrfToken)
                    });

                    client.DefaultRequestHeaders.Add("origin", UriExtensions.SsoBaseUri.ToStringWithoutLastSlash());
                    client.DefaultRequestHeaders.Add("referer", signinUri.ToString());

                    var response = client.PostAsync(signinUri, formContent).GetAwaiter().GetResult();
                    var content = GetResponseContent(response).GetAwaiter().GetResult();
                    ValidateCookiePresence(cookieContainer, "GARMIN-SSO-GUID");
                    var ticket = GetValueByPattern(content, @"var response_url(\s+)= (\""|\').*?ticket=([\w\-]+)(\""|\')", 5, 3);

                    // 3. Second auth step
                    // Needs a service ticket from previous response
                    client.DefaultRequestHeaders.Remove("origin");

                    var authUri = SigninConnection.GetAuthUri(ticket);
                    response = client.GetAsync(authUri).GetAwaiter().GetResult();
                    ValidateModernTicketUrlResponseMessage(response);

                    // Check session cookie
                    ValidateCookiePresence(cookieContainer, "SESSIONID");

                    // Check login
                    response = client.GetAsync(UriExtensions.PrivacityStatementUri).GetAwaiter().GetResult();
                    content = GetResponseContent(response).GetAwaiter().GetResult();
                }
            }
            

            Console.WriteLine("Hello World!");
        }

        private static async Task<string> GetResponseContent(HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();

            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return string.Empty;
                // For activities without GPS coordinates, there is no GPX download (204 = no content).
                // Write an empty file to prevent redownloading it.
            }
            else if (response.StatusCode == HttpStatusCode.OK)
            {
                return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
            else
            {
                return string.Empty;
            }
        }

        private static void ValidateModernTicketUrlResponseMessage(HttpResponseMessage responseMessage)
        {
            if (!responseMessage.IsSuccessStatusCode && !responseMessage.StatusCode.Equals(HttpStatusCode.MovedPermanently))
            {
                throw new Exception("Second auth step failed to produce success or expected 302");
            }
        }

        private static void ValidateCookiePresence(CookieContainer container, string cookieName)
        {
            var cookies = container.GetCookies(UriExtensions.ModernUri).Cast<Cookie>().ToList();
            if (!cookies.Any(e => string.Equals(cookieName, e.Name, StringComparison.InvariantCultureIgnoreCase)))
            {
                throw new Exception($"Missing cookie {cookieName}");
            }
        }

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
    }
}
