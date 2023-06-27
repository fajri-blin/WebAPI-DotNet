﻿using API.Utilities;

namespace API.DTOs.Booking;

public class UpdateBookingDto
{
    public Guid Guid { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public StatusEnums Status { get; set; }
    public string Remarks { get; set; }
    public Guid RoomGuid { get; set; }
    public Guid EmployeeGuid { get; set; }
}