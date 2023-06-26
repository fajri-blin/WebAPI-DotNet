using API.Models;
using API.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoleController : ControllerBase
{
    private readonly IRoleRepository _roleRepository;

    public RoleController(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var entities = _roleRepository.GetAll();

        if (!entities.Any())
        {
            return NotFound();
        }

        return Ok(entities);
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var entity = _roleRepository.GetByGuid(guid);
        if (entity is null)
        {
            return NotFound();
        }
        return Ok(entity);
    }

    [HttpPost]
    public IActionResult Create(Role role)
    {
        var created = _roleRepository.Create(role);
        return Ok(created);
    }

    [HttpPut]
    public IActionResult Update(Role role)
    {
        var isUpdated = _roleRepository.Update(role);
        if (!isUpdated)
        {
            return NotFound();
        }
        return Ok(isUpdated);
    }


}
