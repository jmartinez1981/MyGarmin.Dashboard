using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace GarminFenixSync.Connectivity.Client.Uris
{
    internal static class QueryStringExtensions
    {
        internal const string ClientIdKey = "clientId";
        internal const string ConnectLegaTermsKey = "connectLegalTerms";
        internal const string ConsumeServiceTicketKey = "consumeServiceTicket";
        internal const string CreateAccountShownKey = "createAccountShown";
        internal const string CssUrlKey = "cssUrl";
        internal const string DisplayNameShownKey =  "displayNameShown";
        internal const string EmbebWidgetKey = "embedWidget";
        internal const string GauthHostKey = "gauthHost";
        internal const string GenerateExtraServiceTicketKey = "generateExtraServiceTicket";
        internal const string GenerateTwoExtraServiceTicketsKey = "generateTwoExtraServiceTickets";
        internal const string GenerateNoServiceTicketKey = "generateNoServiceTicket";
        internal const string GlobalOptInCheckedKey = "globalOptInChecked";

        private static readonly Dictionary<string, string> Parameters = new Dictionary<string, string>
        {
            {ClientIdKey, "GarminConnect"},
            {ConnectLegaTermsKey, "true"},
            {ConsumeServiceTicketKey, "false"},
            {CreateAccountShownKey, "true"},
            {CssUrlKey, GarminConnect.CssUri.ToString()},
            {DisplayNameShownKey, "false"},
            {EmbebWidgetKey, "false"},
            {GauthHostKey, GarminSso.SsoUri.ToString()},
            {GenerateExtraServiceTicketKey, "true"},
            {GenerateTwoExtraServiceTicketsKey, "false"},
            {GenerateNoServiceTicketKey, "false"},
            {GlobalOptInCheckedKey, "false"},
            //{"globalOptInShown", "true"},
            //{"id", "gauth-widget"},
            //{"initialFocus", "true"},
            //{"locale", LOCALE},
            //{"locationPromptShon", "true"},
            //{"mobile", "false"},
            //{"openCreateAccount", "false"},
            //{"privacyStatementUrl", PRIVACY_STATEMENT_URL},
            //{"redirectAfterAccountCreationUrl", CONNECT_URL_MODERN},
            //{"redirectAfterAccountLoginUrl", CONNECT_URL_MODERN},
            //{"rememberMeChecked", "false"},
            //{"rememberMeShown", "true"},
            //{"service", CONNECT_URL_MODERN},
            //{"showTermsOfUse", "false"},
            //{"showPrivacyPolicy", "false"},
            //{"showConnectLegalAge", "false"},
            //{"showPassword", "true"},
            //{"source", CONNECT_URL_SIGNIN},
            //{"useCustomHeader", "false"},
            //{"webhost", CONNECT_URL_MODERN}
        };

        public static Uri GetQueryString()
        {
            var uri = GarminSso.SigninUri;
            
            Parameters.ToList().ForEach(x=> uri.AddParameter(x.Key, x.Value));

            return uri;
        }
    }
}
