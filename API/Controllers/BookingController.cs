using Microsoft.AspNetCore.Mvc;
using API.Services;
using API.DTOs.Booking;
using API.Utilities.Handler;
using System.Net;
using API.Utilities.Enum;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = $"{nameof(RoleLevel.Admin)}")]
public class BookingController : ControllerBase
{
    private readonly BookingService _booking;

    public BookingController(BookingService service)
    {
        _booking = service;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var entities = _booking.GetBooking();

        if (entities is null)
        {
            return NotFound(new ResponseHandler<GetBookingDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found"
            });
        }

        return Ok(new ResponseHandler<IEnumerable<GetBookingDto>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data Found",
            Data = entities
        });
    }

    [HttpGet("GetBookingDuration")]
    public IActionResult GetBookingDuration()
    {
        var entities = _booking.BookingDuration();

        if (entities == null || !entities.Any())
        {
            return NotFound(new ResponseHandler<GetBookingDurationDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found"
            });
        }

        return Ok(new ResponseHandler<IEnumerable<GetBookingDurationDto>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data Found",
            Data = entities
        });
    }
    [HttpGet("GetBookingBy")]
    public IActionResult GetBookingBy() 
    {
        var entities = _booking.GetBookingByAll();

        if (entities is null)
        {
            return NotFound(new ResponseHandler<GetBookingByDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found"
            });
        }

        return Ok(new ResponseHandler<IEnumerable<GetBookingByDto>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data Found",
            Data = entities
        });
    }

    [HttpGet("GetBookingBy/{guid}")]
    public IActionResult GetBookingBy(Guid guid)
    {
        var entity = _booking.GetBookingByPerson(guid);
        if (entity is null)
        {
            return NotFound(new ResponseHandler<GetBookingByDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found"
            });
        }

        return Ok(new ResponseHandler<GetBookingByDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data Found",
            Data = entity
        });
    }

    [HttpGet("BookingToday")]
    public IActionResult GetBookingToday()
    {
        var entities = _booking.GetBookingToday();

        if (entities is null)
        {
            return NotFound(new ResponseHandler<GetBookingTodayDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found"
            });
        }

        return Ok(new ResponseHandler<IEnumerable<GetBookingTodayDto>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data Found",
            Data = entities
        });
    }

    [HttpGet("BookingNotToday")]
    public IActionResult GetBookingNotToday()
    {
        var entities = _booking.GetBookingNotToday();

        if (entities is null)
        {
            return NotFound(new ResponseHandler<GetBookingNotTodayDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found"
            });
        }

        return Ok(new ResponseHandler<IEnumerable<GetBookingNotTodayDto>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data Found",
            Data = entities
        });
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var entity = _booking.GetBooking(guid);
        if (entity is null)
        {
            return NotFound(new ResponseHandler<GetBookingDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found"
            });
        }

        return Ok(new ResponseHandler<GetBookingDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data Found",
            Data = entity
        });
    }

    [Authorize(Roles =$"{nameof(RoleLevel.Manager)}, {nameof(RoleLevel.User)}")]
    [HttpPost]
    public IActionResult Create(NewBookingDto Booking)
    {
        var created = _booking.CreateBooking(Booking);
        if (created is null)
        {
            return BadRequest(new ResponseHandler<GetBookingDto>
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Message = "Data not Created"
            });
        }
        return Ok(new ResponseHandler<GetBookingDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully created",
            Data = created
        });
    }

    [HttpPut]
    public IActionResult Update(UpdateBookingDto Booking)
    {
        var isUpdated = _booking.UpdateBooking(Booking);
        if (isUpdated is -1)
        {
            return NotFound(new ResponseHandler<UpdateBookingDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found"
            });
        }
        if (isUpdated is 0)
        {
            return BadRequest(new ResponseHandler<UpdateBookingDto>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Check your data"
            });
        }
        return Ok(new ResponseHandler<UpdateBookingDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully updated"
        });
    }

    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        var delete = _booking.DeleteBooking(guid);

        if (delete is -1)
        {
            return NotFound(new ResponseHandler<GetBookingDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Id not found"
            });
        }
        if (delete is 0)
        {
            return BadRequest(new ResponseHandler<GetBookingDto>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Check connection to database"
            });
        }

        return Ok(new ResponseHandler<GetBookingDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully deleted"
        });
    }

}
