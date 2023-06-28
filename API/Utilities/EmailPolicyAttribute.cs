using API.Services;
using System.ComponentModel.DataAnnotations;

namespace API.Utilities;

public class EmailPolicyAttribute : ValidationAttribute
{
    private readonly EmployeeService _employeeService;
    // Check if email that user inputed is not available yet in Employee Table
    public override bool IsValid(object? value)
    {
        if (value == null)
        {
            return false;
        }

        string email = value.ToString();

        var employee = _employeeService.GetEmployee();

        foreach (var item in employee)
        {
            if (item.Email == email)
            {
                return false;
            }
        }
        ErrorMessage = "Email is not available";

        return true;
    
    }
}
