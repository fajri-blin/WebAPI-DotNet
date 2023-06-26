using API.Models;
using API.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomController : ControllerBase
{
    private readonly IRoomRepository _roomRepository;

    public RoomController(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var entities = _roomRepository.GetAll();

        if (!entities.Any())
        {
            return NotFound();
        }

        return Ok(entities);
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var entity = _roomRepository.GetByGuid(guid);
        if (entity is null)
        {
            return NotFound();
        }
        return Ok(entity);
    }

    [HttpPost]
    public IActionResult Create(Room room)
    {
        var created = _roomRepository.Create(room);
        return Ok(created);
    }

    [HttpPut]
    public IActionResult Update(Room room)
    {
        var isUpdated = _roomRepository.Update(room);
        if (!isUpdated)
        {
            return NotFound();
        }
        return Ok(isUpdated);
    }

    [HttpDelete]
    public IActionResult Delete(Guid guid)
    {
        var isDeleted = _roomRepository.Delete(guid);
        if (!isDeleted)
        {
            return NotFound();
        }
        return Ok();
    }
}
