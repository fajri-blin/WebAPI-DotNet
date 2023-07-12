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
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Text.Json;
using API.Utilities.Handler;
using API.Repositories;

namespace API.Services;

public class AccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUniversityRepository _universityRepository;
    private readonly IEducationRepository _educationRepository;
    private readonly IEmployeeRepository _employeesRepository;
    private readonly IAccountRoleRepository _accountRoleRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly ITokenHandlers _tokenHandler;
    private readonly IEmailHandler _emailHandler;
    private readonly BookingDBContext _dBContext;

    public AccountService(
        IAccountRepository repository, 
        IUniversityRepository universityRepository, 
        IEducationRepository educationRepository, 
        IAccountRoleRepository accountRoleRepository, 
        IEmployeeRepository employeesRepository, 
        IRoleRepository roleRepository,
        BookingDBContext dBContext,
        IEmailHandler emailHandler,
        ITokenHandlers tokenHandler)
    {
        _accountRepository = repository;
        _universityRepository = universityRepository;
        _educationRepository = educationRepository;
        _employeesRepository = employeesRepository;
        _accountRoleRepository = accountRoleRepository;
        _roleRepository = roleRepository;
        _dBContext = dBContext;
        _emailHandler = emailHandler;
        _tokenHandler = tokenHandler;
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
            ExpiredTime = entity.ExpiredTime,
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
            ExpiredTime = entity.ExpiredTime
        };

        return toDto;
    }

    public int ForgotPassword(ForgotPasswordDto forgotPasswordDto)
    {
        var employee = _employeesRepository.GetEmailorPhoneNumber(forgotPasswordDto.Email);
        if (employee is null)
            return 0; // Email not found

        var account = _accountRepository.GetByGuid(employee.Guid);
        if (account is null)
            return -1;

        var otp = new Random().Next(111111, 999999);
        var isUpdated = _accountRepository.Update(new Account
        {
            Guid = account.Guid,
            Password = account.Password,
            IsDeleted = account.IsDeleted,
            OTP = otp,
            ExpiredTime = DateTime.Now.AddMinutes(5),
            IsUsed = false,
            CreatedDate = account.CreatedDate,
            ModifiedDate = DateTime.Now
        });

        if (!isUpdated)
            return -1;

        _emailHandler.SendEmail(forgotPasswordDto.Email,
                                "Forgot Password",
                                $"Your OTP is {otp}");

        return 1;
    }

    public string LoginAccount(LoginDto login)
    {
        var employee = _employeesRepository.CheckEmail(login.email);
        if (employee is null)
        {
            return "0";
        }

        var account = _accountRepository.GetByGuid(employee.Guid);
        if (account is null)
        {
            return "0";
        }

        if(!Hashing.ValidatePassword(login.password, account!.Password))
        {
            return "-1";
        }

        var getAccountRole = _accountRoleRepository.GetByAccountGuid(account.Guid);
        var listRole = (from ar in getAccountRole
                       join r in _roleRepository.GetAll() on ar.RoleGuid equals r.Guid
                       select r.Name);

        var claims = new List<Claim>()
            {
                new Claim("NIK", employee.NIK),
                new Claim("Fullname", $"{employee.FirstName} {employee.LastName}"),
                new Claim("Email", login.email),
            };
        claims.AddRange(listRole.Select(role => new Claim(ClaimTypes.Role, role)));

        try
        {
            var getToken = _tokenHandler.GenerateToken(claims);
            return getToken;
        }
        catch
        {
            return "-2";
        }
    }

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
                BirthDate = newEntity.BirthDate,
                HiringDate = newEntity.HiringDate,
                Email = newEntity.Email,
                NIK = GenerateHandler.GenerateNIK(_employeesRepository, newEntity.NIK),
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
                ExpiredTime = DateTime.Now.AddYears(5),
            };
            _accountRepository.Create(account);

            var universityEntity = _universityRepository.GetByCodeandName(newEntity.UniversityCode, newEntity.UniversityName);
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

            var roleAccount = new AccountRole
            {
                Guid = new Guid(),
                AccountGuid = account.Guid,
                RoleGuid = _roleRepository.GetUser()?.Guid,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
            _accountRoleRepository.Create(roleAccount);


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
            ExpiredTime = newEntity.ExpiredTime,
            
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
            ExpiredTime = created.ExpiredTime
            
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
            ExpiredTime = entity.ExpiredTime,
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

    public int ChangePassword(ChangePasswordDto changePasswordDto)
    {
        var isExist = _employeesRepository.CheckEmail(changePasswordDto.Email);
        if (isExist is null)
        {
            return -1;
        }

        var getAccount = _accountRepository.GetByGuid(isExist.Guid);
        if (getAccount.OTP != changePasswordDto.OTP)
        {
            return 0;
        }

        if (getAccount.IsUsed == true)
        {
            return 1;
        }

        if (getAccount.ExpiredTime < DateTime.Now)
        {
            return 2;
        }

        var account = new Account
        {
            Guid = getAccount.Guid,
            IsUsed = getAccount.IsUsed,
            IsDeleted = getAccount.IsDeleted,
            ModifiedDate = DateTime.Now,
            CreatedDate = getAccount!.CreatedDate,
            OTP = getAccount.OTP,
            ExpiredTime = getAccount.ExpiredTime,
            Password = Hashing.HashPassword(changePasswordDto.NewPassword),
        };

        var isUpdate = _accountRepository.Update(account);
        if (!isUpdate)
        {
            return 0;
        }

        return 3;
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
