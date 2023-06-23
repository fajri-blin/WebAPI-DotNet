using API.Models;
using API.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController : GeneralController<IEmployeeRepository, Employee>
{
    public EmployeeController(IEmployeeRepository employeeRepository) : base(employeeRepository) { }
}
