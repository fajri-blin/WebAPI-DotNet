using API.Data;
using API.Models;
using API.Contracts;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public class UniversityRepository : GeneralRepository<University>, IUniversityRepository
{
    public UniversityRepository(BookingDBContext dbContext) : base(dbContext) { }

    public IEnumerable<University> GetByName(string name)
    {
        return _context.Set<University>().Where(x => x.Name == name);
    }

    public University? GetByCodeandName(string code, string name)
    {
        return _context.Set<University>().FirstOrDefault(University => University.Code.ToLower() == code.ToLower() && University.Name.ToLower() == name.ToLower());
    }
}
