using Microsoft.AspNetCore.Mvc;
using API.Contracts;
using API.Models;
using API.Repositories;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountRoleController : GeneralController<IAccountRoleRepository, AccountRole>
{
    public AccountRoleController(IAccountRoleRepository accountRoleRepository) : base(accountRoleRepository)
    {
    }
}

