using API.Data;
using API.Models;
using API.Contracts;

namespace API.Repositories;

public class UniversityRepository : IUniversityRepository
{
    private readonly BookingDBContext _context;

    public UniversityRepository(BookingDBContext context)
    {
        _context = context;
    }

    public ICollection<University> GetAll()
    {
        return _context.Set<University>().ToList();
    }

    public University? GetByGuid(Guid guid)
    {
        return _context.Set<University>().Find(guid);
    }

    public University Create(University university)
    {
        try
        {
            _context.Set<University>().Add(university);
            _context.SaveChanges();
            return university;
        }
        catch
        {
            return null;
        }
    }

    public bool Update(University university)
    {
        try
        {
            _context.Set<University>().Update(university);
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool Delete(Guid guid)
    {
        try
        {
            var entity = GetByGuid(guid);
            if (entity is null)
            {
                return false;
            }

            _context.Set<University>().Remove(entity);
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
