using API.Models;
using API.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountRoleController : ControllerBase
{
    private readonly IAccountRoleRepository _accountRoleRepository;

    public AccountRoleController(IAccountRoleRepository accountRoleRepository)
    {
        _accountRoleRepository = accountRoleRepository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var entities = _accountRoleRepository.GetAll();

        if (!entities.Any())
        {
            return NotFound();
        }

        return Ok(entities);
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var entity = _accountRoleRepository.GetByGuid(guid);
        if (entity is null)
        {
            return NotFound();
        }
        return Ok(entity);
    }

    [HttpPost]
    public IActionResult Create(AccountRole account_role)
    {
        var created = _accountRoleRepository.Create(account_role);
        return Ok(created);
    }

    [HttpPut]
    public IActionResult Update(AccountRole account_role)
    {
        var isUpdated = _accountRoleRepository.Update(account_role);
        if (!isUpdated)
        {
            return NotFound();
        }
        return Ok(isUpdated);
    }


}
