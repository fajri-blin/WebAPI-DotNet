using API.Data;
using API.Models;
using API.Contracts;

namespace API.Repositories;

public class RoleRepository: GeneralRepository<Role>, IRoleRepository
{
    public RoleRepository(BookingDBContext dbContext) : base(dbContext) { }


    public Role? GetUser()
    {
        string roleName = "User";
        return _context.Set<Role>().FirstOrDefault(role => role.Name == roleName);
    }
}
