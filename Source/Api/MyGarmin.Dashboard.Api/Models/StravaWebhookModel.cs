using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MyGarmin.Dashboard.Api.Models
{
    public class StravaWebhookModel
    {
        [JsonPropertyName("aspect_type")]
        public string EventType { get; set; }

        [JsonPropertyName("event_time")]
        public uint EventTime { get; set; }

        [JsonPropertyName("object_id")]
        public uint ObjectId { get; set; }

        [JsonPropertyName("object_type")]
        public string ObjectType { get; set; }

        [JsonPropertyName("owner_id")]
        public uint AthleteId { get; set; }

        [JsonPropertyName("subscription_id")]
        public int SubscriptionId { get; set; }

        [JsonPropertyName("updates")]
        public Dictionary<string, string> Updates { get; set; }
    }
}
