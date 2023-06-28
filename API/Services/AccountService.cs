using API.Contracts;
using API.DTOs.Account;
using API.DTOs.University;
using API.Models;
using System.Security.Principal;
using API.Utilities;
using API.DTOs.Register;
using System.ComponentModel.DataAnnotations;

namespace API.Services;

public class AccountService
{
    private readonly IAccountRepository _servicesRepository;
    private readonly IUniversityRepository _universityRepository;
    private readonly IEducationRepository _educationRepository;
    private readonly IEmployeeRepository _employeesRepository;

    public AccountService(IAccountRepository repository, IUniversityRepository universityRepository, IEducationRepository educationRepository, IEmployeeRepository employeesRepository)
    {
        _servicesRepository = repository;
        _universityRepository = universityRepository;
        _educationRepository = educationRepository;
        _employeesRepository = employeesRepository;
    }

    public IEnumerable<GetAccountDto>? GetAccount()
    {
        var entities = _servicesRepository.GetAll();
        if(!entities.Any()) 
        {
            return null;
        }

        var Dto = entities.Select(entity => new GetAccountDto
        {
            Guid = entity.Guid,
            IsDeleted = entity.IsDeleted,
            IsUsed = entity.IsUsed,
            ExpiredTime = entity.ExpiredDate,
            Password = entity.Password,
        }).ToList();
        return Dto;
    }

    public GetAccountDto? GetAccount(Guid guid)
    {
        var entity = _servicesRepository.GetByGuid(guid);
        if (entity is null)
        {
            return null;
        }

        var toDto = new GetAccountDto
        {
            Guid = entity.Guid,
            IsDeleted = entity.IsDeleted,
            IsUsed = entity.IsUsed,
            ExpiredTime = entity.ExpiredDate
        };

        return toDto;
    }

    public GetRegisterDto? Register(NewRegisterDto newEntity)
    {
        var employee = new Employee {
            Guid = new Guid(),
            FirstName = newEntity.FirstName,
            LastName = newEntity.LastName,
            Gender = newEntity.Gender,
            HiringDate = newEntity.HiringDate,
            Email = newEntity.Email,
            PhoneNumber = newEntity.PhoneNumber,
        };
        var createdEmployee = _employeesRepository.Create(employee);
        if (createdEmployee is null)
        {
            return null;
        }

        var university = new University{
            Code = newEntity.UniversityCode,
            Name = newEntity.UniversityName,
            Guid = new Guid(),
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now,
        };
        var createdUniversity = _universityRepository.Create(university);
        if (createdUniversity is null)
        {
            return null;
        }
        var education = new Education{
            Guid = new Guid(),
            Degree = newEntity.Degree,
            Major = newEntity.Major,
            GPA = newEntity.GPA,
        };
        var createdEducation = _educationRepository.Create(education);
        if (createdEducation is null)
        {
            return null;
        }

        var account = new Account{
            Guid = employee.Guid,
            Password = Hashing.HashPassword(newEntity.Password),
            IsDeleted = false,
            IsUsed = true,
            OTP = 0,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now,
            ExpiredDate = new DateTime(0000, 0, 0),
        };
        var createdAccount = _servicesRepository.Create(account);
        if (createdAccount is null)
        {
            return null;
        }

        var dto = new GetRegisterDto{
            Guid = createdAccount.Guid,
            Email = createdEmployee.Email,
        };
        return dto;


        // Check if the email is already registered
        // var email = _employeesRepository.GetAll();
        // var isEmailExist = email.Any(x => x.Email == newEntity.Email);
        // if (isEmailExist)
        // {
        //     return null;
        // }

        // Generate NIK attribute using the EmployeeService

        // Create an account entity

        // Save the account entity to the repository
    }




    public GetAccountDto? CreateAccount(NewAccountDto newEntity)
    {
        var entityAccount = new Account
        {
            Guid = newEntity.Guid,
            Password = Hashing.HashPassword(newEntity.Password),
            IsDeleted = newEntity.IsDeleted,
            IsUsed = newEntity.IsUsed,
            OTP = newEntity.OTP,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now,
            ExpiredDate = newEntity.ExpiredTime,
            
        };

        var created = _servicesRepository.Create(entityAccount);
        if (created is null)
        {
            return null;
        }

        var Dto = new GetAccountDto
        {
            Guid = created.Guid,
            Password = created.Password,
            IsDeleted = created.IsDeleted,
            IsUsed = created.IsUsed,
            ExpiredTime = created.ExpiredDate
            
        };

        return Dto;
    }

    public int UpdateAccount(UpdateAccountDto entity) 
    {
        var isExist = _servicesRepository.IsExist(entity.Guid);
        if (!isExist)
        {
            return -1;
        }

        var getEntity = _servicesRepository.GetByGuid(entity.Guid);

        var account = new Account
        {
            Guid = entity.Guid,
            Password = Hashing.HashPassword(entity.Password),
            IsUsed = entity.IsUsed,
            IsDeleted = entity.IsDeleted,
            ExpiredDate = entity.ExpiredTime,
            ModifiedDate = DateTime.Now,
            CreatedDate = getEntity!.CreatedDate
        };

        var isUpdate = _servicesRepository.Update(account);
        if (!isUpdate)
        {
            return 0;
        }

        return 1;
    }

    public int DeleteAccount(Guid guid)
    {
        var isExist = (_servicesRepository.IsExist(guid));
        if (!isExist)
        {
            return -1;
        }

        var account = _servicesRepository.GetByGuid(guid);
        var isDelete = _servicesRepository.Delete(account!);

        if (!isDelete)
        {
            return 0;
        }

        return 1;
    }
}
