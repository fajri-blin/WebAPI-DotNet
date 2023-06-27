using API.Contracts;
using API.DTOs.AccountRole ;

using API.Models;
using System.Security.Principal;

namespace API.Services;

public class AccountRoleService
{
    private readonly IAccountRoleRepository _servicesRepository;

    public AccountRoleService(IAccountRoleRepository entityRepository)
    {
        _servicesRepository = entityRepository;
    }

    public IEnumerable<GetAccountRoleDto>? GetAccountRole()
    {
        var entities = _servicesRepository.GetAll();
        if(!entities.Any()) 
        {
            return null;
        }

        var Dto = entities.Select(entity => new GetAccountRoleDto
        {
            Guid = entity.Guid,
            AccountGuid = entity.AccountGuid,
            RoleGuid = entity.RoleGuid,
        }).ToList();
        return Dto;
    }

    public GetAccountRoleDto? GetAccountRole(Guid guid)
    {
        var entity = _servicesRepository.GetByGuid(guid);
        if (entity is null)
        {
            return null;
        }

        var toDto = new GetAccountRoleDto
        {
            Guid = entity.Guid,
            AccountGuid = entity.AccountGuid,
            RoleGuid = entity.RoleGuid,
        };

        return toDto;
    }

    public GetAccountRoleDto? CreateAccountRole(NewAccountRoleDto newEntity)
    {
        var entity = new AccountRole
        {
            Guid = new Guid(),
            AccountGuid = newEntity.AccountGuid,
            RoleGuid = newEntity.RoleGuid,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };

        var created = _servicesRepository.Create(entity);
        if (created is null)
        {
            return null;
        }

        var Dto = new GetAccountRoleDto
        {
            Guid = created.Guid,
            AccountGuid = created.AccountGuid,
            RoleGuid = created.RoleGuid,
        };

        return Dto;
    }

    public int UpdateAccountRole(UpdateAccountRoleDto updateEntity) 
    {
        var isExist = _servicesRepository.IsExist(updateEntity.Guid);
        if (!isExist)
        {
            return -1;
        }

        var getEntity = _servicesRepository.GetByGuid(updateEntity.Guid);

        var entity = new AccountRole
        {
            Guid = updateEntity.Guid,
            AccountGuid = updateEntity.AccountGuid,
            RoleGuid = updateEntity.RoleGuid,
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

    public int DeleteAccountRole(Guid guid)
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
