using API.Data;
using API.Models;
using API.Contracts;

namespace API.Repositories;

public class RoomRepository : GeneralRepository<Room>, IRoomRepository
{
    public RoomRepository(BookingDBContext dbContext) : base(dbContext) { }
}
