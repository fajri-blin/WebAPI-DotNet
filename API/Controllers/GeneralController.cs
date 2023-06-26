using API.Models;
using API.Contracts;
using Microsoft.AspNetCore.Mvc;
using API.Utilities;
using System.Net;

namespace API.Controllers;

public class GeneralController<TIEntityRepository, TEntity> : ControllerBase
    where TIEntityRepository : IGeneralRepository<TEntity>
    where TEntity : class
{
    protected readonly TIEntityRepository _entityRepository;

    public GeneralController(TIEntityRepository entity_repository)
    {
        _entityRepository = entity_repository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var entities = _entityRepository.GetAll();

        if (!entities.Any())
        {
            return NotFound(new ResponseHandler<TEntity>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "No Data Found"
            });
        }

        return Ok(new ResponseHandler<IEnumerable<TEntity>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = $"Data {typeof(TEntity).Name} Found",
            Data = entities
        });
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var entity = _entityRepository.GetByGuid(guid);
        if (entity is null)
        {
            return NotFound(new ResponseHandler<TEntity>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = $"No {typeof(TEntity).Name} Data Found"
            });
        }
        return Ok(new ResponseHandler<TEntity>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = $"Data {typeof(TEntity).Name} Found",
            Data = entity
        });
    }

    [HttpPost]
    public IActionResult Create(TEntity entity)
    {
        var created = _entityRepository.Create(entity);

        return Ok(new ResponseHandler<TEntity>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = $"Data {typeof(TEntity).Name} Created",
            Data = created
        });
    }

    [HttpPut]
    public IActionResult Update(TEntity entity)
    {
        var isUpdated = _entityRepository.Update(entity);
        if (!isUpdated)
        {
            return NotFound(new ResponseHandler<TEntity>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "No Data Found"
            });
        }
        return Ok(new ResponseHandler<TEntity>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = $"Data {typeof(TEntity).Name} has been Updated",
            Data = entity
        });
    }

    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        var isDeleted = _entityRepository.Delete(guid);
        if (!isDeleted)
        {
            return NotFound(new ResponseHandler<TEntity>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "No Data Found"
            });
        }
        return Ok(new ResponseHandler<TEntity>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = $"Data {typeof(TEntity).Name} Has Been Deleted",
        });
    }
}
