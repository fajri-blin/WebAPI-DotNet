using API.Utilities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models.Tables;

[Table("tb_tr_bookings")]
public class Booking : BaseEntity
{
    [Key]

    [Column("start_date")]
    public DateTime StartDate { get; set; }

    [Column("end_date")]
    public DateTime EndDate { get; set; }

    [Column("remarks", TypeName = "nvarchar(255)")]
    public string? Remarks { get; set; }

    [Column("status")]
    public StatusEnums Status { get; set; }

    [Column("room_guid")]
    public Guid RoomGuid { get; set; }

    [Column("employee_guid")]
    public Guid EmployeeGuid { get; set; }

    //Cardinality
    public Employee Employee { get; set; }
    public Room Room { get; set; }
}
