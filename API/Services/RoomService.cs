using API.Contracts;
using API.DTOs.Role;
using API.DTOs.Room ;

using API.Models;
using System.Data;
using System.Security.Principal;

namespace API.Services;

public class RoomService
{
    private readonly IRoomRepository _servicesRepository;

    public RoomService(IRoomRepository entityRepository)
    {
        _servicesRepository = entityRepository;
    }

    public IEnumerable<GetRoomDto>? GetRoom()
    {
        var entities = _servicesRepository.GetAll();
        if(!entities.Any()) 
        {
            return null;
        }

        var Dto = entities.Select(entity => new GetRoomDto
        {
            Guid = entity.Guid,
            Name = entity.Name,
            Capacity = entity.Capacity,
            Floor = entity.Floor,
        }).ToList();
        return Dto;
    }

    public GetRoomDto? GetRoom(Guid guid)
    {
        var entity = _servicesRepository.GetByGuid(guid);
        if (entity is null)
        {
            return null;
        }

        var toDto = new GetRoomDto
        {
            Guid = entity.Guid,
            Name = entity.Name,
            Capacity = entity.Capacity,
            Floor = entity.Floor,
        };

        return toDto;
    }

    public GetRoomDto? CreateRoom(NewRoomDto newEntity)
    {
        var entity = new Room
        {
            Guid = new Guid(),
            Name = newEntity.Name,
            Capacity = newEntity.Capacity,
            Floor = newEntity.Floor,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };

        var created = _servicesRepository.Create(entity);
        if (created is null)
        {
            return null;
        }

        var Dto = new GetRoomDto
        {
            Guid = entity.Guid,
            Name = entity.Name,
            Capacity = entity.Capacity,
            Floor = entity.Floor,
        };

        return Dto;
    }

    public int UpdateRoom(UpdateRoomDto updateEntity) 
    {
        var isExist = _servicesRepository.IsExist(updateEntity.Guid);
        if (!isExist)
        {
            return -1;
        }

        var getEntity = _servicesRepository.GetByGuid(updateEntity.Guid);

        var entity = new Room
        {
            Guid = updateEntity.Guid,
            Name = updateEntity.Name,
            Capacity = updateEntity.Capacity,
            Floor = updateEntity.Floor,
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

    public int DeleteRoom(Guid guid)
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
