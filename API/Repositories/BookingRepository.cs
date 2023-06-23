using API.Data;
using API.Models;
using API.Contracts;

namespace API.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly BookingDBContext _context;

    public BookingRepository(BookingDBContext context)
    {
        _context = context;
    }

    public ICollection<Booking> GetAll()
    {
        return _context.Set<Booking>().ToList();
    }

    public Booking? GetByGuid(Guid guid)
    {
        return _context.Set<Booking>().Find(guid);
    }

    public Booking Create(Booking booking)
    {
        try
        {
            _context.Set<Booking>().Add(booking);
            _context.SaveChanges();
            return booking;
        }
        catch
        {
            return null;
        }
    }

    public bool Update(Booking booking)
    {
        try
        {
            _context.Set<Booking>().Update(booking);
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

            _context.Set<Booking>().Remove(entity);
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
