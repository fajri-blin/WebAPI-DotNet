using API.Contracts;
using API.DTOs.Education ;

using API.Models;
using System.Security.Principal;

namespace API.Services;

public class EducationService
{
    private readonly IEducationRepository _educationRepository;

    public EducationService(IEducationRepository entityRepository)
    {
        _educationRepository = entityRepository;
    }

    public IEnumerable<GetEducationDto>? GetEducation()
    {
        var entities = _educationRepository.GetAll();
        if(!entities.Any()) 
        {
            return null;
        }

        var Dto = entities.Select(entity => new GetEducationDto
        {
            Guid = entity.Guid,
            Major = entity.Major,
            Degree = entity.Degree,
            GPA = entity.GPA,
            UniversityGuid = entity.UniversityGuid
        }).ToList();
        return Dto;
    }

    public GetEducationDto? GetEducation(Guid guid)
    {
        var entity = _educationRepository.GetByGuid(guid);
        if (entity is null)
        {
            return null;
        }

        var toDto = new GetEducationDto
        {
            Guid = entity.Guid,
            Major = entity.Major,
            Degree = entity.Degree,
            GPA = entity.GPA,
            UniversityGuid = entity.UniversityGuid
        };

        return toDto;
    }

    public GetEducationDto? CreateEducation(NewEducationDto newEntity)
    {
        var entity = new Education
        {
            Guid = new Guid(),
            Major = newEntity.Major,
            Degree = newEntity.Degree,
            GPA = newEntity.GPA,
            UniversityGuid = newEntity.UniversityGuid,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };

        var created = _educationRepository.Create(entity);
        if (created is null)
        {
            return null;
        }

        var Dto = new GetEducationDto
        {
            Guid = entity.Guid,
            Major = entity.Major,
            Degree = entity.Degree,
            GPA = entity.GPA,
            UniversityGuid = entity.UniversityGuid
        };

        return Dto;
    }

    public int UpdateEducation(UpdateEducationDto updateEntity) 
    {
        var isExist = _educationRepository.IsExist(updateEntity.Guid);
        if (!isExist)
        {
            return -1;
        }

        var getEntity = _educationRepository.GetByGuid(updateEntity.Guid);

        var entity = new Education
        {
            Guid = updateEntity.Guid,
            Major = updateEntity.Major,
            Degree = updateEntity.Degree,
            GPA = updateEntity.GPA,
            UniversityGuid = updateEntity.UniversityGuid,
            ModifiedDate = DateTime.Now,
            CreatedDate = getEntity!.CreatedDate
        };

        var isUpdate = _educationRepository.Update(entity);
        if (!isUpdate)
        {
            return 0;
        }

        return 1;
    }

    public int DeleteEducation(Guid guid)
    {
        var isExist = (_educationRepository.IsExist(guid));
        if (!isExist)
        {
            return -1;
        }

        var account = _educationRepository.GetByGuid(guid);
        var isDelete = _educationRepository.Delete(account!);

        if (!isDelete)
        {
            return 0;
        }

        return 1;
    }
}
