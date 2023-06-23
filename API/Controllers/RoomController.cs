using API.Models;
using API.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomController : GeneralController<IRoomRepository, Room>
{
    public RoomController(IRoomRepository roomRepository) : base(roomRepository) { }
}
