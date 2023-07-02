using API.Models;

namespace API.Contracts;

public interface IEmployeeRepository : IGeneralRepository<Employee>
{
    Employee? GetEmailorPhoneNumber(string data);

    IEnumerable<Employee>? GetsByEmail(string email);

    Employee? CheckEmail(string email);
}
