using API.Utilities.Validation;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Account
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

    }
}
