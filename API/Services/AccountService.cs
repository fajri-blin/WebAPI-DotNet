﻿using API.Contracts;
using API.DTOs.Account;
using API.DTOs.University;
using API.Models;
using System.Security.Principal;

namespace API.Services;

public class AccountService
{
    private readonly IAccountRepository _servicesRepository;

    public AccountService(IAccountRepository repository)
    {
        _servicesRepository = repository;
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

    public GetAccountDto? CreateAccount(NewAccountDto newEntity)
    {
        var entityAccount = new Account
        {
            Guid = newEntity.Guid,
            Password = newEntity.Password,
            IsDeleted = newEntity.IsDeleted,
            IsUsed = newEntity.IsUsed,
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
