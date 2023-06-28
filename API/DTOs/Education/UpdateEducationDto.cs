using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Education;

public class UpdateEducationDto
{
    public Guid Guid { get; set; }
    public string Major { get; set; }
    public string Degree { get; set; }

    [Range(0, 4, ErrorMessage = "GPA must range 0 to 4")]
    public double GPA { get; set; }
    public Guid UniversityGuid { get; set; }
}
