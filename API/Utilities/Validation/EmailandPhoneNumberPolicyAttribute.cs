using API.Contracts;
using System.ComponentModel.DataAnnotations;

namespace API.Utilities.Validation;

// This used to check if Email or Phone Number is Duplicated with existing Data in Table Employee
public class EmailandPhoneNumberPolicyAttribute : ValidationAttribute
{
    private readonly string _guidPropertyName;
    private readonly string _propertyName;

    public EmailandPhoneNumberPolicyAttribute(string guidPropertyName, string propertyName)
    {
        _guidPropertyName = guidPropertyName;
        _propertyName = propertyName;
    }

    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success;
        }

        var employeeRepository = (IEmployeeRepository)validationContext.GetService(typeof(IEmployeeRepository));
        var guidPropertyName = validationContext.ObjectType.GetProperty(_guidPropertyName);
        var guidPropertyValue = guidPropertyName?.GetValue(validationContext.ObjectInstance, null) as Guid?;

        var entity = employeeRepository.GetEmailorPhoneNumber(value.ToString());
        if (entity is null)
        {
            return ValidationResult.Success;
        }

        return (entity.Guid == guidPropertyValue ? ValidationResult.Success :
                                                new ValidationResult($"{_propertyName} '{value}' already exists."))!;
    }
}
