using API.Models;
using API.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EducationController : ControllerBase
{
    private readonly IEducationRepository _educationRepository;

    public EducationController(IEducationRepository educationRepository)
    {
        _educationRepository = educationRepository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var entities = _educationRepository.GetAll();

        if (!entities.Any())
        {
            return NotFound();
        }

        return Ok(entities);
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid) 
    {
        var entity = _educationRepository.GetByGuid(guid);
        if(entity is null)
        {
            return NotFound();
        }
        return Ok(entity);
    }

    [HttpPost]
    public IActionResult Create(Education education)
    {
        var created = _educationRepository.Create(education);
        return Ok(created);
    }

    [HttpPut]
    public IActionResult Update(Education education)
    {
        var isUpdated = _educationRepository.Update(education);
        if(!isUpdated)
        {
            return NotFound();
        }
        return Ok(isUpdated);
    }

    [HttpDelete]
    public IActionResult Delete(Guid guid)
    {
        var isDeleted = _educationRepository.Delete(guid);
        if(!isDeleted)
        {
            return NotFound();
        }
        return Ok();
    }
}
