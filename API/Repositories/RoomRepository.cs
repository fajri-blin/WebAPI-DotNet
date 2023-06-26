using API.Data;
using API.Models;
using API.Contracts;

namespace API.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly BookingDBContext _context;

    public RoomRepository(BookingDBContext context)
    {
        _context = context;
    }

    public ICollection<Room> GetAll()
    {
        return _context.Set<Room>().ToList();
    }

    public Room? GetByGuid(Guid guid)
    {
        return _context.Set<Room>().Find(guid);
    }

    public Room Create(Room room)
    {
        try
        {
            _context.Set<Room>().Add(room);
            _context.SaveChanges();
            return room;
        }
        catch
        {
            return null;
        }
    }

    public bool Update(Room room)
    {
        try
        {
            _context.Set<Room>().Update(room);
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool Delete(Guid guid)
    {
        try
        {
            var entity = GetByGuid(guid);
            if (entity is null)
            {
                return false;
            }

            _context.Set<Room>().Remove(entity);
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
