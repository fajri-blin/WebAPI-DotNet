using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Account;

public class LoginDto
{
    [Required]
    [EmailAddress]
    public string email { get; set; }
    [Required]
    public string password { get; set; }
}
