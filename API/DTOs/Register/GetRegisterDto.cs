using API.Utilities;

namespace API.DTOs.Register;

public class GetRegisterDto
{
    public Guid Guid { get; set; }
    public string Email { get; set; }
}
