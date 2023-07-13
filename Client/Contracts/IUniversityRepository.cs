using API.DTOs.University;
using API.Models;
using API.Utilities.Handler;

namespace Client.Contracts;

public interface IUniversityRepository : IGeneralRepository<University>
{
    Task<ResponseHandler<GetUniversityDto>> Put(Guid guid, GetUniversityDto universityDto);
}
