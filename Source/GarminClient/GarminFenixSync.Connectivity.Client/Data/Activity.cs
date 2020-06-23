using MyGarmin.Connectivity.Client.Data.Converters;
using System;
using System.Text.Json.Serialization;

namespace MyGarmin.Connectivity.Client.Data
{
    public class Activity
    {
        [JsonPropertyName("activityId")]
        public long ActivityId { get; set; }

        [JsonPropertyName("activityName")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("startTimeGMT")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime StartTimeUTC { get; set; }

        [JsonPropertyName("startTimeLocal")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime StartTimeLocal { get; set; }

        [JsonPropertyName("calories")]
        public float Calories { get; set; }

        [JsonPropertyName("averageHR")]
        [JsonConverter(typeof(CustomFloatConverter))]
        public float HeartRateAvg { get; set; }

        [JsonPropertyName("maxHR")]
        [JsonConverter(typeof(CustomFloatConverter))]
        public float HeartRateMax { get; set; }
    }
}
