using System.Text.Json.Serialization;

namespace MyGarmin.Dashboard.Connectivity.StravaClient.Data
{
    public class ShoeInfo
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("primary")]
        public bool Default { get; set; }

        [JsonPropertyName("name")]
        public bool Name { get; set; }

        [JsonPropertyName("distance")]
        public float Distance { get; set; }
    }
}
