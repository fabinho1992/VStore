using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace UserApi.Domain.Interfaces.IAuthService
{
    public interface ITokenService
    {
        JwtSecurityToken GenerationToken(IEnumerable<Claim> claims, IConfiguration _config);
    }
}
