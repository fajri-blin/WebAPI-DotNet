using API.Data;
using API.Models;
using API.Contracts;
using System.Security.Principal;

namespace API.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly BookingDBContext _context;

    public AccountRepository(BookingDBContext context)
    {
        _context = context;
    }

    public ICollection<Account> GetAll()
    {
        return _context.Set<Account>().ToList();
    }

    public Account? GetByGuid(Guid guid)
    {
        return _context.Set<Account>().Find(guid);
    }

    public Account Create(Account account)
    {
        try
        {
            _context.Set<Account>().Add(account);
            _context.SaveChanges();
            return account;
        }
        catch
        {
            return null;
        }
    }

    public bool Update(Account account)
    {
        try
        {
            _context.Set<Account>().Update(account);
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

            _context.Set<Account>().Remove(entity);
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
