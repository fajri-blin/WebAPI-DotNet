﻿using API.Utilities;

namespace API.DTOs.Employee;

public class UpdateEmployeeDto
{
    public Guid Guid { get; set; }
    public string NIK { get; set; }
    public string FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public GenderEnum Gender { get; set; }
    public DateTime HiringDate { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}