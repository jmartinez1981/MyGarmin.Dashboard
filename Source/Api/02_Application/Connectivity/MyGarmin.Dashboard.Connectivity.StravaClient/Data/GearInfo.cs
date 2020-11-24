using System.Text.Json.Serialization;

namespace MyGarmin.Dashboard.Connectivity.StravaClient.Data
{
    public class GearInfo
    {
        [JsonPropertyName("id")]
        public string GearId { get; set; }

        [JsonPropertyName("brand_name")]
        public string Brand { get; set; }

        [JsonPropertyName("model_name")]
        public string Model { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}
