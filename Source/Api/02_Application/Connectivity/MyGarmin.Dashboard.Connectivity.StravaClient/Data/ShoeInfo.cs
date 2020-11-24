using MyGarmin.Dashboard.Connectivity.StravaClient.Data.Converters;
using System.Text.Json.Serialization;

namespace MyGarmin.Dashboard.Connectivity.StravaClient.Data
{
    public class ShoeInfo
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("primary")]
        [JsonConverter(typeof(CustomBoolConverter))]
        public bool Default { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("distance")]
        public float Distance { get; set; }
    }
}
