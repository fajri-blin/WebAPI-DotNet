/*using API.Contracts;
using API.DTOs.Register;

namespace API.Temp;

public class RegisterService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IEducationRepository _educationRepository;
    private readonly IUniversityRepository _universityRepository;
    private readonly IAccountRepository _accountRepository;

    public RegisterService(IEmployeeRepository employeeRepository, IEducationRepository educationRepository, IUniversityRepository universityRepository, IAccountRepository accountRepository)
    {
        _employeeRepository = employeeRepository;
        _educationRepository = educationRepository;
        _universityRepository = universityRepository;
        _accountRepository = accountRepository;
    }

    public IEnumerable<GetRegisterDto> GetRegisterDtos()
    {
        var employees = _employeeRepository.GetAll();
        var educations = _educationRepository.GetAll();
        var universities = _universityRepository.GetAll();
        var account = _accountRepository.GetAll();

        //LINQ
        var result = from employee in employees
                      join education in educations on employee.Guid equals education.Guid
                      join university in universities on education.UniversityGuid equals university.Guid
                      join account in account on employee.Guid equals account.Guid
                      select new GetRegisterDto
                      {
                          FirstName = employee.FirstName,
                          LastName = employee.LastName,
                          BirthDate = employee.BirthDate,
                          Gender = employee.Gender,
                          HiringDate = employee.HiringDate,
                          Email = employee.Email,
                          PhoneNumber = employee.PhoneNumber,
                          Major = education.Major,
                          Degree = education.Degree,
                          GPA = education.GPA,
                          UniversityCode = university.Code,
                          UniversityName = university.Name,
                          Password = account.Password,
                          ConfirmPassword = account.Password
                      };

        return result;

    }
}
*/