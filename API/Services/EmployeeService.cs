using API.Contracts;
using API.DTOs.Employee ;

using API.Models;
using API.Repositories;
using API.Utilities;
using API.Utilities.Handler;
using System.Security.Principal;

namespace API.Services;

public class EmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IEducationRepository _educationRepository;
    private readonly IUniversityRepository _universityRepository;

    public EmployeeService(IEmployeeRepository entityRepository, 
                           IEducationRepository educationRepository,
                           IUniversityRepository universityRepository)
    {
        _employeeRepository = entityRepository;
        _educationRepository = educationRepository;
        _universityRepository = universityRepository;
    }

    public IEnumerable<GetEmployeeDto>? GetEmployee()
    {
        var entities = _employeeRepository.GetAll();
        if(!entities.Any()) 
        {
            return null;
        }

        var Dto = entities.Select(entity => new GetEmployeeDto
        {
            Guid = entity.Guid,
            NIK = entity.NIK,
            BirthDate = entity.BirthDate,
            Email = entity.Email,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Gender = entity.Gender,
            HiringDate = entity.HiringDate,
            PhoneNumber = entity.PhoneNumber

        }).ToList();
        return Dto;
    }

    public GetEmployeeDto? GetEmployee(Guid guid)
    {
        var entity = _employeeRepository.GetByGuid(guid);
        if (entity is null)
        {
            return null;
        }

        var toDto = new GetEmployeeDto
        {
            Guid = entity.Guid,
            NIK = entity.NIK,
            BirthDate = entity.BirthDate,
            Email = entity.Email,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Gender = entity.Gender,
            HiringDate = entity.HiringDate,
            PhoneNumber = entity.PhoneNumber
        };

        return toDto;
    }

    public IEnumerable<GetEmployeeMasterDto>? GetEmployeeMasters()
    {
        var Dto = (from employee in _employeeRepository.GetAll()
                       join education in _educationRepository.GetAll() on employee.Guid equals education.Guid
                       join university in _universityRepository.GetAll() on education.UniversityGuid equals university.Guid into universityGroup
                       from university in universityGroup.DefaultIfEmpty()
                       select new GetEmployeeMasterDto
                       {
                           EmployeeGuid = employee.Guid,
                           NIK = employee.NIK,
                           FullName = $"{employee.FirstName} {employee.LastName}",
                           BirthDate = employee.BirthDate,
                           Gender = employee.Gender,
                           Email = employee.Email,
                           PhoneNumber = employee.PhoneNumber,
                           Major = education.Major,
                           Degree = education.Degree,
                           GPA = education. GPA,
                           UniversityName = university != null ? university.Name : null
                       }).ToList();
        return Dto;
    }
    public GetEmployeeMasterDto? GetEmployeeMastersByGuid(Guid guid)
    {
        var employee = _employeeRepository.GetByGuid(guid);
        if (employee == null) return null;

        var education = _educationRepository.GetByGuid(employee.Guid);
        if (education == null) return null;

        University? university = null;
        if (education.UniversityGuid.HasValue)
        {
            university = _universityRepository.GetByGuid(education.UniversityGuid.Value);
        }

        var Dto = new GetEmployeeMasterDto
        {
            EmployeeGuid = employee.Guid,
            NIK = employee.NIK,
            FullName = $"{employee.FirstName} {employee.LastName}",
            BirthDate = employee.BirthDate,
            Gender = employee.Gender,
            Email = employee.Email,
            PhoneNumber = employee.PhoneNumber,
            Major = education.Major,
            Degree = education.Degree,
            GPA = education.GPA,
            UniversityName = university != null ? university.Name : null
        };

        return Dto;
    }




    public GetEmployeeDto? CreateEmployee(NewEmployeeDto newEntity)
    {
        var entity = new Employee
        {
            Guid = new Guid(),
            PhoneNumber = newEntity.PhoneNumber,
            FirstName = newEntity.FirstName,
            LastName = newEntity.LastName,
            Gender = newEntity.Gender,
            HiringDate = newEntity.HiringDate,
            Email = newEntity.Email,
            BirthDate = newEntity.BirthDate,
            NIK = GenerateHandler.GenerateNIK(_employeeRepository, newEntity.NIK),
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };

        var created = _employeeRepository.Create(entity);
        if (created is null)
        {
            return null;
        }

        var Dto = new GetEmployeeDto
        {
            Guid = entity.Guid,
            NIK = entity.NIK,
            BirthDate = entity.BirthDate,
            Email = entity.Email,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Gender = entity.Gender,
            HiringDate = entity.HiringDate,
            PhoneNumber = entity.PhoneNumber
        };

        return Dto;
    }

    public int UpdateEmployee(UpdateEmployeeDto updateEntity) 
    {
        var isExist = _employeeRepository.IsExist(updateEntity.Guid);
        if (!isExist)
        {
            return -1;
        }

        var getEntity = _employeeRepository.GetByGuid(updateEntity.Guid);

        var entity = new Employee
        {
            Guid = updateEntity.Guid,
            PhoneNumber = updateEntity.PhoneNumber,
            FirstName = updateEntity.FirstName,
            LastName = updateEntity.LastName,
            Gender = updateEntity.Gender,
            HiringDate = updateEntity.HiringDate,
            Email = updateEntity.Email,
            BirthDate = updateEntity.BirthDate,
            NIK = updateEntity.NIK,
            ModifiedDate = DateTime.Now,
            CreatedDate = getEntity!.CreatedDate
        };

        var isUpdate = _employeeRepository.Update(entity);
        if (!isUpdate)
        {
            return 0;
        }

        return 1;
    }

    public int DeleteEmployee(Guid guid)
    {
        var isExist = (_employeeRepository.IsExist(guid));
        if (!isExist)
        {
            return -1;
        }

        var account = _employeeRepository.GetByGuid(guid);
        var isDelete = _employeeRepository.Delete(account!);

        if (!isDelete)
        {
            return 0;
        }

        return 1;
    }
}
