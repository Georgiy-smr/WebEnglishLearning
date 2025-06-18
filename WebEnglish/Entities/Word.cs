using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Word : Entity
    {
        [Required]
        [MinLength(1, ErrorMessage = "EnglishWord cannot be empty")]
        public string EngWord { get; set; } = string.Empty;
        [Required]
        [MinLength(1, ErrorMessage = "EnglishWord cannot be empty")]
        public string RusWord { get; set; } = string.Empty;
    }
}
