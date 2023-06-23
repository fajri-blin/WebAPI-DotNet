using API.Data;
using API.Models;
using API.Contracts;

namespace API.Repositories;

public class AccountRoleRepository : GeneralRepository<AccountRole>,IAccountRoleRepository
{
    public AccountRoleRepository(BookingDBContext dbContext) : base(dbContext) { }
}
