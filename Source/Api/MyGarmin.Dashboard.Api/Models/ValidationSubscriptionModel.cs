using System.Text.Json.Serialization;

namespace MyGarmin.Dashboard.Api.Models
{
    public class ValidationSubscriptionModel
    {
        [JsonPropertyName("hub.challenge")]
        public string Challenge { get; set; }
    }
}
