using MyGarmin.Dashboard.Connectivity.StravaClient.Data.Converters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MyGarmin.Dashboard.Connectivity.StravaClient.Data
{
    public class Athlete
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("firstname")]
        public string Firstname { get; set; }

        [JsonPropertyName("lastname")]
        public string Lastname { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("coutry")]
        public string Country { get; set; }

        [JsonPropertyName("profile")]
        [JsonConverter(typeof(CustomUriConverter))]
        public Uri ProfileImage { get; set; }

        [JsonPropertyName("profile_medium")]
        [JsonConverter(typeof(CustomUriConverter))]
        public Uri ProfileImageSmall { get; set; }
    }
}
