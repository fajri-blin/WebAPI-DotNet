using API.Contracts;
using API.DTOs.Account;
using API.DTOs.University;
using API.Models;
using System.Security.Principal;
using API.Utilities;
using API.DTOs.Auth;
using System.ComponentModel.DataAnnotations;
using API.Data;
using System.Transactions;

namespace API.Services;

public class AccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUniversityRepository _universityRepository;
    private readonly IEducationRepository _educationRepository;
    private readonly IEmployeeRepository _employeesRepository;
    private readonly BookingDBContext _dBContext;

    public AccountService(BookingDBContext dBContext,IAccountRepository repository, IUniversityRepository universityRepository, IEducationRepository educationRepository, IEmployeeRepository employeesRepository)
    {
        _accountRepository = repository;
        _universityRepository = universityRepository;
        _educationRepository = educationRepository;
        _employeesRepository = employeesRepository;
        _dBContext = dBContext;
    }

    public IEnumerable<GetAccountDto>? GetAccount()
    {
        var entities = _accountRepository.GetAll();
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
        var entity = _accountRepository.GetByGuid(guid);
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
    public string GenerateNIK(string nik)
    {
        var entities = _employeesRepository.GetAll();
        if (entities is null)
        {
            return "11111";
        }

        if (string.IsNullOrEmpty(nik))
        {
            if (int.TryParse(entities.Last().NIK, out int lastNIK))
            {
                return (lastNIK + 1).ToString();
            }
        }

        return nik;
    }

    /*    public int ChangePassword(ChangePassword changePassword)
        {
            var IsExist = _employeesRepository.
        }
    */
    public GetRegisterDto? Register(NewRegisterDto newEntity)
    {
        using var transaction = _dBContext.Database.BeginTransaction();
        try
        {
            var employee = new Employee
            {
                Guid = new Guid(),
                FirstName = newEntity.FirstName,
                LastName = newEntity.LastName ?? "",
                Gender = newEntity.Gender,
                HiringDate = newEntity.HiringDate,
                Email = newEntity.Email,
                NIK = GenerateNIK(newEntity.NIK),
                PhoneNumber = newEntity.PhoneNumber,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
            };
            _employeesRepository.Create(employee);

            var account = new Account
            {
                Guid = employee.Guid,
                Password = Hashing.HashPassword(newEntity.Password),
                IsDeleted = false,
                IsUsed = false,
                OTP = 0,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                ExpiredDate = DateTime.Now.AddYears(5),
            };
            _accountRepository.Create(account);

            var universityEntity = _universityRepository.GetByCode(newEntity.UniversityCode);
            if (universityEntity == null)
            {
                var university = new University
                {
                    Code = newEntity.UniversityCode,
                    Name = newEntity.UniversityName,
                    Guid = new Guid(),
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                };
                universityEntity = _universityRepository.Create(university);
            }

            var education = new Education
            {
                Guid = employee.Guid,
                Degree = newEntity.Degree,
                Major = newEntity.Major,
                GPA = newEntity.GPA,
                UniversityGuid = universityEntity.Guid,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
            };
            _educationRepository.Create(education);

            var dto = new GetRegisterDto
            {
                Guid = employee.Guid,
                Email = employee.Email,
            };

            transaction.Commit();
            return dto;
        }
        catch
        {
            transaction.Rollback();
            return null;
        }
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

        var created = _accountRepository.Create(entityAccount);
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
        var isExist = _accountRepository.IsExist(entity.Guid);
        if (!isExist)
        {
            return -1;
        }

        var getEntity = _accountRepository.GetByGuid(entity.Guid);

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

        var isUpdate = _accountRepository.Update(account);
        if (!isUpdate)
        {
            return 0;
        }

        return 1;
    }

    public int DeleteAccount(Guid guid)
    {
        var isExist = (_accountRepository.IsExist(guid));
        if (!isExist)
        {
            return -1;
        }

        var account = _accountRepository.GetByGuid(guid);
        var isDelete = _accountRepository.Delete(account!);

        if (!isDelete)
        {
            return 0;
        }

        return 1;
    }
}
