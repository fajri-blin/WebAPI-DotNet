using API.Models;
using API.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingController : GeneralController<IBookingRepository, Booking>
{
    public BookingController(IBookingRepository bookingRepository) : base(bookingRepository) { }
}
