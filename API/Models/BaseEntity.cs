using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models;

public abstract class BaseEntity
{
    [Column("guid")]
    public Guid Guid { get; set; }

    [Column("created_date")]
    public DateTime CreatedDate { get; set; }

    [Column("modified_date")]
    public DateTime ModifiedDate { get; set; }
}
