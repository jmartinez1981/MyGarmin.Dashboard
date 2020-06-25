using System;
using System.Collections.Generic;
using System.Linq;

namespace TestConnection
{
    public static class SigninConnection
    {
        private static readonly Dictionary<string, string> QueryStringParameters = new Dictionary<string, string>
        {
            {"clientId", "GarminConnect"},
            {"connectLegalTerms", "true"},
            {"consumeServiceTicket", "false"},
            {"createAccountShown", "true"},
            {"cssUrl", UriExtensions.CssUri.ToString()},
            {"displayNameShown", "false"},
            {"embedWidget", "false"},
            {"gauthHost", UriExtensions.SsoUri.ToString()},
            {"generateExtraServiceTicket", "true"},
            {"generateTwoExtraServiceTickets", "false"},
            {"generateNoServiceTicket", "false"},
            {"globalOptInChecked", "false"},
            {"globalOptInShown", "true"},
            {"id", "gauth-widget"},
            {"initialFocus", "true"},
            {"locale", "en_US"},
            {"locationPromptShon", "true"},
            {"mobile", "false"},
            {"openCreateAccount", "false"},
            {"privacyStatementUrl", UriExtensions.PrivacityStatementUri.ToString()},
            {"redirectAfterAccountCreationUrl", UriExtensions.ModernUri.ToString()},
            {"redirectAfterAccountLoginUrl", UriExtensions.ModernUri.ToString()},
            {"rememberMeChecked", "false"},
            {"rememberMeShown", "true"},
            {"service", UriExtensions.ModernUri.ToString()},
            {"showTermsOfUse", "false"},
            {"showPrivacyPolicy", "false"},
            {"showConnectLegalAge", "false"},
            {"showPassword", "true"},
            {"source", UriExtensions.SigninUri.ToString()},
            {"useCustomHeader", "false"},
            {"webhost", UriExtensions.ModernUri.ToString()}
        };
                
        public static Uri GetSsoSigninUri()
        {
            var uri = UriExtensions.SsoSigninUri;
            QueryStringParameters.ToList().ForEach(x => uri = uri.AddParameter(x.Key, x.Value));

            return uri;
        }

        public static Uri GetAuthUri(string ticket)
        {
            return UriExtensions.ModernUri.AddParameter("ticket", ticket);
        }
    }
}
