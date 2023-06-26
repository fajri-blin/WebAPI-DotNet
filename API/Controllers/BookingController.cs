using API.Models;
using API.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingController : ControllerBase
{
    private readonly IBookingRepository _bookingRepository;

    public BookingController(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var entities = _bookingRepository.GetAll();

        if (!entities.Any())
        {
            return NotFound();
        }

        return Ok(entities);
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var entity = _bookingRepository.GetByGuid(guid);
        if (entity is null)
        {
            return NotFound();
        }
        return Ok(entity);
    }

    [HttpPost]
    public IActionResult Create(Booking booking)
    {
        var created = _bookingRepository.Create(booking);
        return Ok(created);
    }

    [HttpPut]
    public IActionResult Update(Booking booking)
    {
        var isUpdated = _bookingRepository.Update(booking);
        if (!isUpdated)
        {
            return NotFound();
        }
        return Ok(isUpdated);
    }


}
