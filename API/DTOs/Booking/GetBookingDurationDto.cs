namespace API.DTOs.Booking;

public class GetBookingDurationDto
{
    public Guid? RoomGuid { get; set; }
    public string RoomName { get; set; }
    public string BookingLength { get; set; }
}
