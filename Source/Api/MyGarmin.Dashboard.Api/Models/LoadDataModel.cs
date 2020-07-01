using System.ComponentModel.DataAnnotations;

namespace MyGarmin.Dashboard.Api.Models
{
    public class LoadDataModel
    {
        [Required]
        public string Type { get; set; }
    }
}
