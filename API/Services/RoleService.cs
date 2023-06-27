using API.Contracts;
using API.DTOs.Role ;

using API.Models;
using System.Data;
using System.Security.Principal;

namespace API.Services;

public class RoleService
{
    private readonly IRoleRepository _servicesRepository;

    public RoleService(IRoleRepository entityRepository)
    {
        _servicesRepository = entityRepository;
    }

    public IEnumerable<GetRoleDto>? GetRole()
    {
        var entities = _servicesRepository.GetAll();
        if(!entities.Any()) 
        {
            return null;
        }

        var Dto = entities.Select(entity => new GetRoleDto
        {
            Guid = entity.Guid,
            Name = entity.Name

        }).ToList();
        return Dto;
    }

    public GetRoleDto? GetRole(Guid guid)
    {
        var entity = _servicesRepository.GetByGuid(guid);
        if (entity is null)
        {
            return null;
        }

        var toDto = new GetRoleDto
        {
            Guid = entity.Guid,
            Name = entity.Name,
        };

        return toDto;
    }

    public GetRoleDto? CreateRole(NewRoleDto newEntity)
    {
        var entity = new Role
        {
            Guid = new Guid(),
            Name = newEntity.Name,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };

        var created = _servicesRepository.Create(entity);
        if (created is null)
        {
            return null;
        }

        var Dto = new GetRoleDto
        {
            Guid = entity.Guid,
            Name = entity.Name
        };

        return Dto;
    }

    public int UpdateRole(UpdateRoleDto updateEntity) 
    {
        var isExist = _servicesRepository.IsExist(updateEntity.Guid);
        if (!isExist)
        {
            return -1;
        }

        var getEntity = _servicesRepository.GetByGuid(updateEntity.Guid);

        var entity = new Role
        {
            Guid = updateEntity.Guid,
            Name = updateEntity.Name,
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

    public int DeleteRole(Guid guid)
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
