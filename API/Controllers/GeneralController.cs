using API.Models;
using API.Contracts;
using Microsoft.AspNetCore.Mvc;

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
            return NotFound();
        }

        return Ok(entities);
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var entity = _entityRepository.GetByGuid(guid);
        if (entity is null)
        {
            return NotFound();
        }
        return Ok(entity);
    }

    [HttpPost]
    public IActionResult Create(TEntity entity)
    {
        var created = _entityRepository.Create(entity);
        return Ok(created);
    }

    [HttpPut]
    public IActionResult Update(TEntity entity)
    {
        var isUpdated = _entityRepository.Update(entity);
        if (!isUpdated)
        {
            return NotFound();
        }
        return Ok(isUpdated);
    }

    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        var isDeleted = _entityRepository.Delete(guid);
        if (!isDeleted)
        {
            return NotFound();
        }
        return Ok();
    }
}
