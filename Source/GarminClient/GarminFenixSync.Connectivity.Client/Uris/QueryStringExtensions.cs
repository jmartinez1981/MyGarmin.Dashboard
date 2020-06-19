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
        internal const string GlobalOptInShownKey = "globalOptInShown";
        internal const string LocaleKey = "locale";
        internal const string IdKey = "id";
        internal const string InitialFocusKey = "initialFocus";
        internal const string LocationPromptShonKey = "locationPromptShon";
        internal const string MobileKey = "mobile";
        internal const string OpenCreateAccountKey = "openCreateAccount";
        internal const string PrivacyStatementUrlKey = "privacyStatementUrl";
        internal const string RedirectAfterAccountCreationUrlKey = "redirectAfterAccountCreationUrl";
        internal const string RedirectAfterAccountLoginUrlKey = "redirectAfterAccountLoginUrl";
        internal const string RememberMeCheckedKey = "rememberMeChecked";
        internal const string RememberMeShownKey = "rememberMeShown";
        internal const string ServiceKey = "service";
        internal const string ShowTermsOfUseKey = "showTermsOfUse";
        internal const string ShowPrivacyPolicyKey = "showPrivacyPolicy";
        internal const string ShowConnectLegalAgeKey = "showConnectLegalAge";
        internal const string ShowPasswordKey = "showPassword";
        internal const string SourceKey = "source";
        internal const string UseCustomHeaderKey = "useCustomHeader";
        internal const string WebhostKey = "webhost";

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
            {GlobalOptInShownKey, "true"},
            {IdKey, "gauth-widget"},
            {InitialFocusKey, "true"},
            {LocaleKey, "LOCALE"},
            {LocationPromptShonKey, "true"},
            {MobileKey, "false"},
            {OpenCreateAccountKey, "false"},
            {PrivacyStatementUrlKey, Garmin.PrivacyStatementUri.ToString()},
            {RedirectAfterAccountCreationUrlKey, GarminConnect.ModernUri.ToString()},
            {RedirectAfterAccountLoginUrlKey, GarminConnect.ModernUri.ToString()},
            {RememberMeCheckedKey, "false"},
            {RememberMeShownKey, "true"},
            {ServiceKey, GarminConnect.ModernUri.ToString()},
            {ShowTermsOfUseKey, "false"},
            {ShowPrivacyPolicyKey, "false"},
            {ShowConnectLegalAgeKey, "false"},
            {ShowPasswordKey, "true"},
            {SourceKey, GarminConnect.SiginUri.ToString()},
            {UseCustomHeaderKey, "false"},
            {WebhostKey, GarminConnect.ModernUri.ToString()}
        };

        public static Uri GetQueryString(Uri uri)
        {
            Parameters.ToList().ForEach(x=> uri.AddParameter(x.Key, x.Value));

            return uri;
        }
    }
}
