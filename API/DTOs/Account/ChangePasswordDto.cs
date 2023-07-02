using API.Utilities.Validation;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Account;

public class ChangePasswordDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public int OTP { get; set; }

    [Required]
    [PasswordPolicy]
    public string NewPassword { get; set; }

    [ConfirmPasswordAttributes("NewPassword", ErrorMessage = "Confirmation Passwird doesn't match with password")]
    public string ConfirmPassword { get; set; }
}
