using System.Text.Json.Serialization;

namespace MyGarmin.Dashboard.ApplicationServices.Entities
{
    public class User
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }

        [JsonIgnore]
        public string Password { get; set; }
    }
}
