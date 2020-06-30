using System.ComponentModel.DataAnnotations;

namespace MyGarmin.Dashboard.Api.Models
{
    public class StravaConnectionCreationModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Token { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string RefreshToken { get; set; }
    }
}
