using API.Data;
using API.Models;
using API.Contracts;

namespace API.Repositories;

public class RoleRepository: IRoleRepository
{
    private readonly BookingDBContext _context;

    public RoleRepository(BookingDBContext context)
    {
        _context = context;
    }

    public ICollection<Role> GetAll()
    {
        return _context.Set<Role>().ToList();
    }

    public Role? GetByGuid(Guid guid)
    {
        return _context.Set<Role>().Find(guid);
    }

    public Role Create(Role role)
    {
        try
        {
            _context.Set<Role>().Add(role);
            _context.SaveChanges();
            return role;
        }
        catch
        {
            return null;
        }
    }

    public bool Update(Role role)
    {
        try
        {
            _context.Set<Role>().Update(role);
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

            _context.Set<Role>().Remove(entity);
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
