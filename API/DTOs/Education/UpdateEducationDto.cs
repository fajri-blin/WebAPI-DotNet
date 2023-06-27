﻿namespace API.DTOs.Education;

public class UpdateEducationDto
{
    public Guid Guid { get; set; }
    public string Major { get; set; }
    public string Degree { get; set; }
    public double Gpa { get; set; }
    public Guid UniversityGuid { get; set; }
}
