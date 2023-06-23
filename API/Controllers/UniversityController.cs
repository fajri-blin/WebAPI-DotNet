using API.Models;
using API.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UniversityController : ControllerBase
{
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
    }
}
