using System.Security.Claims;

namespace API.Contracts;

public interface ITokenHandlers
{
    string GenerateToken(IEnumerable<Claim> claims);
}
