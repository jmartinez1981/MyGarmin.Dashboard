using MyGarmin.Dashboard.Connectivity.StravaClient.Data.Converters;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MyGarmin.Dashboard.Connectivity.StravaClient.Data
{
    public class AthleteInfo
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

        [JsonPropertyName("follower_count")]
        public int FollowerCount { get; set; }

        [JsonPropertyName("friend_count")]
        public int FriendCount { get; set; }

        [JsonPropertyName("measurement_preference")]
        public string MeasurementPreference { get; set; }

        [JsonPropertyName("shoes")]
        public List<ShoeInfo> Shoes { get; set; }
    }
}
