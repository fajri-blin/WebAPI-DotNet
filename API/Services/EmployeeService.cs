using API.Contracts;
using API.DTOs.Employee ;

using API.Models;
using API.Utilities;
using System.Security.Principal;

namespace API.Services;

public class EmployeeService
{
    private readonly IEmployeeRepository _servicesRepository;

    public EmployeeService(IEmployeeRepository entityRepository)
    {
        _servicesRepository = entityRepository;
    }

    public IEnumerable<GetEmployeeDto>? GetEmployee()
    {
        var entities = _servicesRepository.GetAll();
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
        var entity = _servicesRepository.GetByGuid(guid);
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

    public string GenerateNIK(string nik)
    {
        var entities = _servicesRepository.GetAll();
        if (entities is null)
        {
            return "11111";
        }
        if (nik == "")
        {
            int LastNIK = Convert.ToInt32(entities.LastOrDefault().NIK);
            return Convert.ToString(LastNIK + 1);
        }
        return nik;
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
            NIK = GenerateNIK(newEntity.NIK),
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };

        var created = _servicesRepository.Create(entity);
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
        var isExist = _servicesRepository.IsExist(updateEntity.Guid);
        if (!isExist)
        {
            return -1;
        }

        var getEntity = _servicesRepository.GetByGuid(updateEntity.Guid);

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

        var isUpdate = _servicesRepository.Update(entity);
        if (!isUpdate)
        {
            return 0;
        }

        return 1;
    }

    public int DeleteEmployee(Guid guid)
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
