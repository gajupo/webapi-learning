using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using webapi_learning.Models.Validations;

namespace webapi_learning.Models
{
    public class Shirt
    {
        [JsonPropertyName("id")]
        [Key]
        public int Id { get; set; }

        [Required]
        [JsonPropertyName("model")]
        public string? Model { get; set; }

        [Required]
        [Shirt_EnsureTheCorrectSizing]
        public int? Size { get; set; }

        public string? Gender { get; set; }
    }
}
