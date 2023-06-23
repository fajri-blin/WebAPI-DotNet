using API.Data;
using API.Models;
using API.Contracts;

namespace API.Repositories;

public class BookingRepository : GeneralRepository<Booking>, IBookingRepository
{
    public BookingRepository(BookingDBContext dbContext) : base(dbContext) { }
}
