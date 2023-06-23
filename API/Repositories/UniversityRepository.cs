using API.Data;
using API.Models;
using API.Contracts;

namespace API.Repositories;

public class UniversityRepository : GeneralRepository<University>, IUniversityRepository
{
    public UniversityRepository(BookingDBContext dbContext) : base(dbContext) { }
}
