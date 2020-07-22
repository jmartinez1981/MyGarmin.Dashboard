using MyGarmin.Dashboard.Connectivity.StravaClient.Data.Converters;
using System;
using System.Text.Json.Serialization;

namespace MyGarmin.Dashboard.Connectivity.StravaClient.Data
{
    public class ExchangeTokenInfo
    {
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        [JsonPropertyName("expires_at")]
        public long ExpiresAt { get; set; }

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
    }
}
