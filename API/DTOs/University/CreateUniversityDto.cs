using Microsoft.Extensions.Primitives;

namespace API.DTOs.University;

public class CreateUniversityDto
{
    public string Code { get; set; }
    public string Name { get; set; }  
}
