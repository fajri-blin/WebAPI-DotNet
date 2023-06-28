using API.Utilities;

namespace API.DTOs.Register;

public class NewRegisterDto
{
    public Guid Guid { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public GenderEnum Gender { get; set; }
    public DateTime HiringDate { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Major { get; set; }
    public string Degree { get; set; }
    public double GPA { get; set; }
    public string UniversityCode { get; set; }
    public string UniversityName { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }

}
