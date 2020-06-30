using System.ComponentModel.DataAnnotations;

namespace MyGarmin.Dashboard.Api.Models
{
    public class StravaConnectionUpdateModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string Token { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string RefreshToken { get; set; }

        public bool LoadData { get; set; }
    }
}
