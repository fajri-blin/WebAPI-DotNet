using API.Contracts;
using API.DTOs.University;
using API.Models;

namespace API.Services;

public class UniversityService
{
    private readonly IUniversityRepository _servicesRepository;

    public UniversityService(IUniversityRepository services)
    {
        _servicesRepository = services;
    }

    public IEnumerable<GetUniversityDto>? GetUniversity()
    {
        var entities = _servicesRepository.GetAll();
        if(!entities.Any()) 
        {
            return null;
        }

        var Dto = entities.Select(entity => new GetUniversityDto
        {
            Guid = entity.Guid,
            Name = entity.Name,
            Code = entity.Code,
        });

        return Dto;
    }

    public IEnumerable<GetUniversityDto>? GetUniversity(string name)
    {
        var entities = _servicesRepository.GetByName(name);
        if (!entities.Any())
        {
            return null;
        }

        var Dto = entities.Select(entity => new GetUniversityDto
        {
            Guid = entity.Guid,
            Name = entity.Name,
            Code = entity.Code,
        });
        return Dto;
    }

    public GetUniversityDto? GetUniversity(Guid guid)
    {
        var entity = _servicesRepository.GetByGuid(guid);
        if (entity is null)
        {
            return null;
        }

        var toDto = new GetUniversityDto
        {
            Guid = entity.Guid,
            Name = entity.Name,
            Code = entity.Code
        };

        return toDto;
    }

    public GetUniversityDto? CreateUniversity(NewUniversityDto newEntity)
    {
        var entityUniversity = new University
        {
            Code = newEntity.Code,
            Name = newEntity.Name,
            Guid = new Guid(),
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };

        var created = _servicesRepository.Create(entityUniversity);
        if (created is null)
        {
            return null;
        }

        var Dto = new GetUniversityDto
        {
            Guid = created.Guid,
            Code = created.Code, 
            Name = created.Name 
        };

        return Dto;
    }

    public int UpdateUniversity(UpdateUniversityDto entity) 
    {
        var isExist = _servicesRepository.IsExist(entity.Guid);
        if (!isExist)
        {
            return -1;
        }

        var getEntity = _servicesRepository.GetByGuid(entity.Guid);

        var entityUniversity = new University
        {
            Guid = entity.Guid,
            Code = entity.Code,
            Name = entity.Name,
            CreatedDate = getEntity!.CreatedDate,
            ModifiedDate = DateTime.Now
        };

        var isUpdate = _servicesRepository.Update(entityUniversity);
        if (!isUpdate)
        {
            return 0;
        }

        return 1;
    }

    public int DeleteUniversity(Guid guid)
    {
        var isExist = (_servicesRepository.IsExist(guid));
        if (!isExist)
        {
            return -1;
        }

        var entityUniversity = _servicesRepository.GetByGuid(guid);
        var isDelete = _servicesRepository.Delete(entityUniversity!);

        if (!isDelete)
        {
            return 0;
        }

        return 1;
    }
}
