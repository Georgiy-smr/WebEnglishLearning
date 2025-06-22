using System.ComponentModel.DataAnnotations;

namespace Entities;

public class User : Entity
{
    [Required]
    [MinLength(1, ErrorMessage = "Name cannot be empty")]
    public string UserName { get; set; } = string.Empty;
    [Required]
    [MinLength(1, ErrorMessage = "Name cannot be empty")]
    public string PasswordHash { get; set; } = string.Empty;
}