﻿using API.Utilities;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Employee;

public class NewEmployeeDto
{
    public string NIK { get; set; }
    [Required]
    public string FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public GenderEnum Gender { get; set; }
    public DateTime HiringDate { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [Phone]
    public string PhoneNumber { get; set; }
}
