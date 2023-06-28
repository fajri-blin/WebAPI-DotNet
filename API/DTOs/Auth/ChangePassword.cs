using API.Utilities.Validation;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Auth;

public class ChangePassword
{
    [Required]
    public string Email { get; set; }

    [Required]
    public int OTP { get; set; }

    [Required]
    [PasswordPolicy]
    public string Password { get; set; }

    [Required]
    [ConfirmPasswordAttributes("Password", ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; }
}
