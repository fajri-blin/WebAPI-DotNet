using API.Data;
using API.Models;
using API.Contracts;

namespace API.Repositories;

public class EducationRepository : IEducationRepository
{
    private readonly BookingDBContext _context;

    public EducationRepository(BookingDBContext context)
    {
        _context = context;
    }

    public ICollection<Education> GetAll()
    {
        return _context.Set<Education>().ToList();
    }

    public Education? GetByGuid(Guid guid)
    {
        return _context.Set<Education>().Find(guid);
    }

    public Education Create(Education education)
    {
        try
        {
            _context.Set<Education>().Add(education);
            _context.SaveChanges();
            return education;
        }
        catch
        {
            return null;
        }
    }

    public bool Update(Education education)
    {
        try
        {
            _context.Set<Education>().Update(education);
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

            _context.Set<Education>().Remove(entity);
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
