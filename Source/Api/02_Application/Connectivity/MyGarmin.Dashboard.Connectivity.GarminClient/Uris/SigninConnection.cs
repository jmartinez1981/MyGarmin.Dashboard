using System;
using System.Collections.Generic;
using System.Linq;

namespace MyGarmin.Dashboard.Connectivity.GarminClient.Uris
{
    public static class SigninConnection
    {
        private const string TicketParameterKey = "ticket";

        private static readonly Dictionary<string, string> QueryStringParameters = new Dictionary<string, string>
        {
            {"clientId", "GarminConnect"},
            {"connectLegalTerms", "true"},
            {"consumeServiceTicket", "false"},
            {"createAccountShown", "true"},
            {"cssUrl", GarminConnectUri.Css.ToString()},
            {"displayNameShown", "false"},
            {"embedWidget", "false"},
            {"gauthHost", GarminSSoUri.Sso.ToString()},
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
            {"privacyStatementUrl", GarminCorporativeUri.Privacy.ToString()},
            {"redirectAfterAccountCreationUrl", GarminConnectUri.Modern.ToString()},
            {"redirectAfterAccountLoginUrl", GarminConnectUri.Modern.ToString()},
            {"rememberMeChecked", "false"},
            {"rememberMeShown", "true"},
            {"service", GarminConnectUri.Modern.ToString()},
            {"showTermsOfUse", "false"},
            {"showPrivacyPolicy", "false"},
            {"showConnectLegalAge", "false"},
            {"showPassword", "true"},
            {"source", GarminConnectUri.Signin.ToString()},
            {"useCustomHeader", "false"},
            {"webhost", GarminConnectUri.Modern.ToString()}
        };
                
        public static Uri GetSsoSigninUri()
        {
            var uri = GarminSSoUri.Signin;
            QueryStringParameters.ToList().ForEach(x => uri = uri.AddParameter(x.Key, x.Value));

            return uri;
        }

        public static Uri GetAuthUri(string ticket)
        {
            return GarminConnectUri.Modern.AddParameter(TicketParameterKey, ticket);
        }
    }
}
