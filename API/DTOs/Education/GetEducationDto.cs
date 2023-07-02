namespace API.DTOs.Education;

public class GetEducationDto
{
    public Guid Guid { get; set; }
    public string Major { get; set; }
    public string Degree { get; set; }
    public double GPA { get; set; }
    public Guid? UniversityGuid { get; set; }
}
