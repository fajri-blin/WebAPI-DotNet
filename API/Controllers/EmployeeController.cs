using API.Models;
using API.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeController(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var entities = _employeeRepository.GetAll();

        if (!entities.Any())
        {
            return NotFound();
        }

        return Ok(entities);
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var entity = _employeeRepository.GetByGuid(guid);
        if (entity is null)
        {
            return NotFound();
        }
        return Ok(entity);
    }

    [HttpPost]
    public IActionResult Create(Employee employee)
    {
        var created = _employeeRepository.Create(employee);
        return Ok(created);
    }

    [HttpPut]
    public IActionResult Update(Employee employee)
    {
        var isUpdated = _employeeRepository.Update(employee);
        if (!isUpdated)
        {
            return NotFound();
        }
        return Ok(isUpdated);
    }

 
}
