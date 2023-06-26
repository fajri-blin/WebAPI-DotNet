using API.Data;
using API.Models;
using API.Contracts;
using System.Security.Principal;

namespace API.Repositories;

public class AccountRepository : GeneralRepository<Account>, IAccountRepository
{
    public AccountRepository(BookingDBContext dbContext) : base(dbContext) { }
}
