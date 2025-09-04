using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FicWriter.API.Infrastructure.Security.Tokens.Access;

public class JwtTokenGenerator(string key, uint expirationTime, string issuer, string audience) : IAccessTokenGenerator
{
    private readonly string _key = key;
    private readonly uint _expirationTime = expirationTime;
    private readonly string _issuer = issuer;
    private readonly string _audience = audience;

    public string Generate(Guid guid)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, guid.ToString())
        };

        var token = new SecurityTokenDescriptor
        {
            Issuer = _issuer,
            Audience = _audience,
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_expirationTime),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key)),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(token);

        return tokenHandler.WriteToken(securityToken);
    }
}
