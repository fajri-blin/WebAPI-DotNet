using API.Utilities.Validation;
using API.Utilities.Enum;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Auth;

public class NewRegisterDto
{
    [Required]
    public string FirstName { get; set; }
    public string LastName { get; set; }

    [Required]
    public DateTime BirthDate { get; set; }

    [Required]
    [Range(0,1)]
    public GenderEnum Gender { get; set; }

    [Required]
    public DateTime HiringDate { get; set; }

    public string NIK { get; set; }


    [Required]
    [EmailAddress]
    [EmailandPhoneNumberPolicy("string", "Email")]
    public string Email { get; set; }

    [Required]
    [Phone]
    [EmailandPhoneNumberPolicy("string", "PhoneNumber")]
    public string PhoneNumber { get; set; }

    [Required]
    public string Major { get; set; }

    [Required]
    public string Degree { get; set; }

    [Required]
    public double GPA { get; set; }

    [Required]
    public string UniversityCode { get; set; }

    [Required]
    public string UniversityName { get; set; }

    [Required]
    [PasswordPolicy]
    public string Password { get; set; }

    [Required]
    [ConfirmPasswordAttributes("Password", ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; }

}
