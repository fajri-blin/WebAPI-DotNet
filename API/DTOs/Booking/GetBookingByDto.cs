using API.Utilities.Enum;

namespace API.DTOs.Booking;

public class GetBookingByDto
{
    public Guid BookingGuid { get; set; }
    public string NIK { get; set; }
    public string BookedBy { get; set; }
    public string RoomName { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public StatusEnums Status { get; set; }
    public string Remarks { get; set; }

}
