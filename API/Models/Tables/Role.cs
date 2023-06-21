using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models.Tables;

[Table("tb_m_roles")]
public class Role : BaseEntity
{
    [Key]

    [Column("name", TypeName = "nvarchar(100)")]
    public string Name { get; set; }

    //Cardinaltiy

    public ICollection<AccountRole> AccountRoles { get; set;}
}
