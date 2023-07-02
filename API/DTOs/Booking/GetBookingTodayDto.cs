using API.Utilities.Enum;

namespace API.DTOs.Booking;

public class GetBookingTodayDto
{
    public Guid BookingGuid { get; set; }
    public string RoomName { get; set; }
    public StatusEnums Status { get; set; }
    public int Floor { get; set; }
    public string BookedBy { get; set; }

}
