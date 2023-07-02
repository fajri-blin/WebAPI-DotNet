using API.Utilities.Enum;

namespace API.DTOs.Booking;

public class GetBookingNotTodayDto
{
    public Guid RoomGuid { get; set; }
    public string RoomName { get; set;}

    public int Floor { get; set; }
    public int Capacity { get; set; }


}
