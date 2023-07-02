using API.Contracts;
using API.DTOs.Booking ;

using API.Models;
using System.Security.Principal;

namespace API.Services;

public class BookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IEmployeeRepository _employeeRepository;

    public BookingService(IBookingRepository entityRepository, 
                          IRoomRepository roomRepository,
                          IEmployeeRepository employeeRepository)
    {
        _bookingRepository = entityRepository;
        _roomRepository = roomRepository;
        _employeeRepository = employeeRepository;
    }

    public IEnumerable<GetBookingDto>? GetBooking()
    {
        var entities = _bookingRepository.GetAll();
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
        var entity = _bookingRepository.GetByGuid(guid);
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

        var created = _bookingRepository.Create(entity);
        if (created is null)
        {
            return null;
        }

        var Dto = new GetBookingDto
        {
            Guid = created.Guid,
            StartDate = created.StartDate,
            EndDate = created.EndDate,
            Status = created.Status,
            Remarks = created.Remarks,
            RoomGuid = created.RoomGuid,
            EmployeeGuid = created.EmployeeGuid,
        };

        return Dto;
    }

    public int UpdateBooking(UpdateBookingDto updateEntity) 
    {
        var isExist = _bookingRepository.IsExist(updateEntity.Guid);
        if (!isExist)
        {
            return -1;
        }

        var getEntity = _bookingRepository.GetByGuid(updateEntity.Guid);

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

        var isUpdate = _bookingRepository.Update(entity);
        if (!isUpdate)
        {
            return 0;
        }

        return 1;
    }

    public IEnumerable<GetBookingTodayDto> GetBookingToday()
    {
        try
        {
            var entities = (from booking in _bookingRepository.GetAll()
                          join employee in _employeeRepository.GetAll() on booking.EmployeeGuid equals employee.Guid
                          join room in _roomRepository.GetAll() on booking.RoomGuid equals room.Guid
                          where booking.StartDate == DateTime.Now
                          select new GetBookingTodayDto
                          {
                              BookingGuid = booking.Guid,
                              RoomName = room.Name,
                              Status = booking.Status,
                              Floor = room.Floor,
                              BookedBy = $"{employee.FirstName} {employee.LastName}"
                          }).ToList();
            return entities;
        }
        catch
        {
            return null;
        }
    }

    public IEnumerable<GetBookingNotTodayDto> GetBookingNotToday()
    {
        try
        {
            var entities = (from booking in _bookingRepository.GetAll()
                            join room in _roomRepository.GetAll() on booking.RoomGuid equals room.Guid
                            where booking.StartDate != DateTime.Now
                            select new GetBookingNotTodayDto
                            {
                                RoomGuid = room.Guid,
                                RoomName = room.Name,
                                Floor = room.Floor,
                                Capacity = room.Capacity,
                            }).ToList();
            return entities;
        }
        catch
        {
            return null;
        }
    }

    public int DeleteBooking(Guid guid)
    {
        var isExist = (_bookingRepository.IsExist(guid));
        if (!isExist)
        {
            return -1;
        }

        var account = _bookingRepository.GetByGuid(guid);
        var isDelete = _bookingRepository.Delete(account!);

        if (!isDelete)
        {
            return 0;
        }

        return 1;
    }

    public IEnumerable<GetBookingByDto>? GetBookingByAll()
    {
        try
        {
            var Dto = (from employee in _employeeRepository.GetAll()
                      join booking in _bookingRepository.GetAll() on employee.Guid equals booking.EmployeeGuid
                      join room in _roomRepository.GetAll() on booking.RoomGuid equals room.Guid
                      select new GetBookingByDto
                      {
                          BookingGuid = booking.Guid,
                          NIK = employee.NIK,
                          BookedBy = $"{employee.FirstName} {employee.LastName}",
                          RoomName = room.Name,
                          StartDate = booking.StartDate,
                          EndDate = booking.EndDate,
                          Status = booking.Status,
                          Remarks = booking.Remarks,
                      }).ToList();

            return Dto.ToList();
        }
        catch
        {
            return null;
        }
    }

    public GetBookingByDto? GetBookingByPerson(Guid guid)
    {
        var booking = _bookingRepository.GetByGuid(guid);
        if (booking == null) { return null; }
        var employee = _employeeRepository.GetByGuid(booking.EmployeeGuid);
        if (employee == null) { return null; };

        Room? room = null;
        if(booking.RoomGuid.HasValue)
        {
            room = _roomRepository.GetByGuid(booking.RoomGuid ?? Guid.Empty);
        }

        var dto = new GetBookingByDto
        {
            BookingGuid = booking.Guid,
            NIK = employee.NIK,
            BookedBy = $"{employee.FirstName} {employee.LastName}",
            RoomName = room?.Name,
            StartDate = booking.StartDate,
            EndDate = booking.EndDate,
            Status = booking.Status,
            Remarks = booking.Remarks,
        };
        return dto;
    }

    public IEnumerable<GetBookingDurationDto> BookingDuration()
    {
        var bookings = _bookingRepository.GetAll();
        var rooms = _roomRepository.GetAll();

        var entities = (from booking in bookings
                        join room in rooms on booking.RoomGuid equals room.Guid
                        select new
                        {
                            guid = room.Guid,
                            startDate = booking.StartDate,
                            endDate = booking.EndDate,
                            roomName = room.Name
                        }).ToList();

        var listBookingDurations = new List<GetBookingDurationDto>();

        foreach (var entity in entities)
        {
            TimeSpan duration = entity.endDate - entity.startDate;

            // Count the number of weekends within the duration
            int totalDays = (int)duration.TotalDays;
            int weekends = 0;

            for (int i = 0; i <= totalDays; i++)
            {
                var currentDate = entity.startDate.AddDays(i);
                if (currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    weekends++;
                }
            }
            
            // Calculate the duration without weekends
            TimeSpan bookingLength = duration - TimeSpan.FromDays(weekends);

            var bookingDurationDto = new GetBookingDurationDto
            {
                RoomGuid = entity.guid,
                RoomName = entity.roomName,
                BookingLength = $"{bookingLength.Days} Hari"
            };

            listBookingDurations.Add(bookingDurationDto);
        }

        return listBookingDurations;
    }



}
