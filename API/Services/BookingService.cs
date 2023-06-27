using API.Contracts;
using API.DTOs.Booking ;

using API.Models;
using System.Security.Principal;

namespace API.Services;

public class BookingService
{
    private readonly IBookingRepository _servicesRepository;

    public BookingService(IBookingRepository entityRepository)
    {
        _servicesRepository = entityRepository;
    }

    public IEnumerable<GetBookingDto>? GetBooking()
    {
        var entities = _servicesRepository.GetAll();
        if(!entities.Any()) 
        {
            return null;
        }

        var Dto = entities.Select(entity => new GetBookingDto
        {
            Guid = entity.Guid,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            Status = entity.Status,
            Remarks = entity.Remarks,
            RoomGuid = entity.RoomGuid,
            EmployeeGuid = entity.EmployeeGuid
        }).ToList();
        return Dto;
    }

    public GetBookingDto? GetBooking(Guid guid)
    {
        var entity = _servicesRepository.GetByGuid(guid);
        if (entity is null)
        {
            return null;
        }

        var toDto = new GetBookingDto
        {
            Guid = entity.Guid,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            Status = entity.Status,
            Remarks = entity.Remarks,
            RoomGuid = entity.RoomGuid,
            EmployeeGuid = entity.EmployeeGuid
        };

        return toDto;
    }

    public GetBookingDto? CreateBooking(NewBookingDto newEntity)
    {
        var entity = new Booking
        {
            Guid = new Guid(),
            StartDate = newEntity.StartDate,
            EndDate = newEntity.EndDate,
            Status = newEntity.Status,
            Remarks = newEntity.Remarks,
            RoomGuid = newEntity.RoomGuid,
            EmployeeGuid = newEntity.EmployeeGuid,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };

        var created = _servicesRepository.Create(entity);
        if (created is null)
        {
            return null;
        }

        var Dto = new GetBookingDto
        {
            Guid = created.Guid,
            StartDate = newEntity.StartDate,
            EndDate = newEntity.EndDate,
            Status = newEntity.Status,
            Remarks = newEntity.Remarks,
            RoomGuid = newEntity.RoomGuid,
            EmployeeGuid = newEntity.EmployeeGuid,
        };

        return Dto;
    }

    public int UpdateBooking(UpdateBookingDto updateEntity) 
    {
        var isExist = _servicesRepository.IsExist(updateEntity.Guid);
        if (!isExist)
        {
            return -1;
        }

        var getEntity = _servicesRepository.GetByGuid(updateEntity.Guid);

        var entity = new Booking
        {
            Guid = updateEntity.Guid,
            StartDate = updateEntity.StartDate,
            EndDate = updateEntity.EndDate,
            Status = updateEntity.Status,
            Remarks = updateEntity.Remarks,
            RoomGuid = updateEntity.RoomGuid,
            EmployeeGuid = updateEntity.EmployeeGuid,
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

    public int DeleteBooking(Guid guid)
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
