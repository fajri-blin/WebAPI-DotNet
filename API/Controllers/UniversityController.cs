using API.Models;
using API.Contracts;
using API.Utilities.Handler;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using API.Services;
using API.DTOs.University;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using API.Utilities.Enum;
using Microsoft.AspNetCore.Authorization;
using System.Data;
namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = $"{nameof(RoleLevel.Admin)}")]
public class UniversityController : ControllerBase
{
    private readonly UniversityService _service;


    public UniversityController(UniversityService service)
    {
        _service = service;
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult GetAll()
    {
        var entities = _service.GetUniversity();

        if (entities is null)
        {
            return NotFound(new ResponseHandler<GetUniversityDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found"
            });
        }

        return Ok(new ResponseHandler<IEnumerable<GetUniversityDto>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data Found",
            Data = entities
        });
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var entity = _service.GetUniversity(guid);
        if (entity is null)
        {
            return NotFound(new ResponseHandler<GetUniversityDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found"
            });
        }
        return Ok(new ResponseHandler<GetUniversityDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data Found",
            Data = entity
        });
    }

    [HttpGet("name/{name}")]
    public IActionResult GetByName(string name)
    {
        var entity = _service.GetUniversity(name);
        if(!entity.Any())
        {
            return NotFound(new ResponseHandler<GetUniversityDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found"
            });
        }
        return Ok(new ResponseHandler<IEnumerable<GetUniversityDto>>
        {
            Code =StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data found",
            Data = entity
        });
    }

    [HttpPost]
    public IActionResult Create(NewUniversityDto university)
    {
        var created = _service.CreateUniversity(university);
        if (created is null)
        {
            return BadRequest(new ResponseHandler<GetUniversityDto>
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Message = "Data not Created"
            });
        }
        return Ok(new ResponseHandler<GetUniversityDto>
        {
            Code =StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully created",
            Data = created
        });

    }

    [HttpPut]
    public IActionResult Update(UpdateUniversityDto university)
    {
        var isUpdated = _service.UpdateUniversity(university);
        if (isUpdated is -1)
        {
            return NotFound(new ResponseHandler<UpdateUniversityDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found"
            });
        }
        if (isUpdated is 0)
        {
            return BadRequest(new ResponseHandler<UpdateUniversityDto>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Check your data"
            });
        }
        return Ok(new ResponseHandler<UpdateUniversityDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully updated"
        });
    }

    [HttpDelete]
    public IActionResult Delete(Guid guid)
    {
        var delete = _service.DeleteUniversity(guid);

        if (delete is -1)
        {
            return NotFound(new ResponseHandler<GetUniversityDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Id not found"
            });
        }
        if (delete is 0)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHandler<GetUniversityDto>()
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Error retrieving data from database"
            });


            return BadRequest(new ResponseHandler<GetUniversityDto>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Check connection to database"
            });
        }

        return Ok(new ResponseHandler<GetUniversityDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully deleted"
        });
    }


}
