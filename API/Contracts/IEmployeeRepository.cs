using API.Models;

namespace API.Contracts;

public interface IEmployeeRepository : IGeneralRepository<Employee>
{
    Employee? GetEmailorPhoneNumber(string data);
}
