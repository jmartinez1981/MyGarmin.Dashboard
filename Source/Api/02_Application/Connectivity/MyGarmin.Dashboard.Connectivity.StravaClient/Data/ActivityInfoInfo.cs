using MyGarmin.Dashboard.Connectivity.StravaClient.Data.Converters;
using System;
using System.Text.Json.Serialization;

namespace MyGarmin.Dashboard.Connectivity.StravaClient.Data
{
    public class ActivityInfo
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("distance")]
        public float Distance { get; set; }

        [JsonPropertyName("total_elevation_gain")]
        public float ElevationGain { get; set; }
        
        [JsonPropertyName("elev_high")]
        public float ElevantionHigh { get; set; }

        [JsonPropertyName("elev_low")]
        public float ElevationLow { get; set; }

        [JsonPropertyName("start_date")]
        public DateTime StartDate { get; set; }

        [JsonPropertyName("moving_time")]
        public int MovingTime { get; set; }

        [JsonPropertyName("elapsed_time")]
        public int ElapsedTime { get; set; }

        [JsonPropertyName("gear_id")]
        public string GearId { get; set; }
    }
}
