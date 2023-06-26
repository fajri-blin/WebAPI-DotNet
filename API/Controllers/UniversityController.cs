using API.Models;
using API.Contracts;
using API.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UniversityController : ControllerBase
{
<<<<<<< HEAD
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
=======
    private readonly IUniversityRepository _universityRepository;

    public UniversityController(IUniversityRepository universityRepository)
    {
        _universityRepository = universityRepository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var entities = _universityRepository.GetAll();

        if (!entities.Any())
        {
            return NotFound();
        }

        return Ok(entities);
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid) 
    {
        var entity = _universityRepository.GetByGuid(guid);
        if(entity is null)
        {
            return NotFound();
        }
        return Ok(entity);
    }

    [HttpPost]
    public IActionResult Create(University university)
    {
        var created = _universityRepository.Create(university);
        return Ok(created);
    }

    [HttpPut]
    public IActionResult Update(University university)
    {
        var isUpdated = _universityRepository.Update(university);
        if(!isUpdated)
        {
            return NotFound();
        }
        return Ok(isUpdated);
    }

    [HttpDelete]
    public IActionResult Delete(Guid guid)
    {
        var isDeleted = _universityRepository.Delete(guid);
        if(!isDeleted)
        {
            return NotFound();
        }
        return Ok();
>>>>>>> parent of ecb12b2 (Refactoring)
    }
}
