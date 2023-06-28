using API.Models;

namespace API.Contracts;

public interface IEmployeeRepository : IGeneralRepository<Employee>
{
    Employee? GetEmailorPhoneNumber(string data);

    IEnumerable<Employee>? GetByEmail(string email);
}
