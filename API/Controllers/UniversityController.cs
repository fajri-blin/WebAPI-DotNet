using API.Models;
using API.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UniversityController : GeneralController<IUniversityRepository, University>
{
    public UniversityController(IUniversityRepository universityRepository) : base(universityRepository) { }

    [HttpGet("name/{name}")]
    public IActionResult GetByName(string name)
    {
        var universityName = _entityRepository.GetByName(name);

        if (universityName is null)
        {
            return NotFound();
        }

        return Ok(universityName);
    }
}
