using API.Data;
using API.Models;
using API.Contracts;

namespace API.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly BookingDBContext _context;

    public EmployeeRepository(BookingDBContext context)
    {
        _context = context;
    }

    public ICollection<Employee> GetAll()
    {
        return _context.Set<Employee>().ToList();
    }

    public Employee? GetByGuid(Guid guid)
    {
        return _context.Set<Employee>().Find(guid);
    }

    public Employee Create(Employee employee)
    {
        try
        {
            _context.Set<Employee>().Add(employee);
            _context.SaveChanges();
            return employee;
        }
        catch
        {
            return null;
        }
    }

    public bool Update(Employee employee)
    {
        try
        {
            _context.Set<Employee>().Update(employee);
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

            _context.Set<Employee>().Remove(entity);
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
