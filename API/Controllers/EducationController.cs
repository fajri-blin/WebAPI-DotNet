using API.Models;
using API.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EducationController : GeneralController<IEducationRepository, Education>
{
    public EducationController(IEducationRepository educationRepository) : base(educationRepository) { }
}
