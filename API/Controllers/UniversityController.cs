using API.Models;
using API.Contracts;
using API.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
            return NotFound(new ResponseHandler<University>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "University Name Not Found"
            });
        }

        return Ok(new ResponseHandler<IEnumerable<University>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = $"{name} Found",
            Data = universityName
        });
    }
}
