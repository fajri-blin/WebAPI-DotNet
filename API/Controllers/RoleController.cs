using API.Models;
using API.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoleController : GeneralController<IRoleRepository,  Role>
{
    public RoleController(IRoleRepository roleRepository) : base(roleRepository) { }
}
