using API.Data;
using API.Models;
using API.Contracts;

namespace API.Repositories;

public class AccountRoleRepository : IAccountRoleRepository
{
    private readonly BookingDBContext _context;

    public AccountRoleRepository(BookingDBContext context)
    {
        _context = context;
    }

    public ICollection<AccountRole> GetAll()
    {
        return _context.Set<AccountRole>().ToList();
    }

    public AccountRole? GetByGuid(Guid guid)
    {
        return _context.Set<AccountRole>().Find(guid);
    }

    public AccountRole Create(AccountRole account_role)
    {
        try
        {
            _context.Set<AccountRole>().Add(account_role);
            _context.SaveChanges();
            return account_role;
        }
        catch
        {
            return null;
        }
    }

    public bool Update(AccountRole account_role)
    {
        try
        {
            _context.Set<AccountRole>().Update(account_role);
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

            _context.Set<AccountRole>().Remove(entity);
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
