using API.Models;

namespace API.Contracts;

public interface IAccountRoleRepository
{
    ICollection<AccountRole> GetAll();
    AccountRole? GetByGuid(Guid guid);
    AccountRole Create(AccountRole account_role);
    bool Update(AccountRole account_role);
    bool Delete(Guid guid);
}
