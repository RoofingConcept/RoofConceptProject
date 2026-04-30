using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RoofingConcept.Business.Results;
using RoofingConcept.Data.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RoofingConcept.Business.Services;

public interface IJwtTokenBuilder
{
    JwtServiceResult BuildJwtToken(ApplicationUserEntity entity);
}

public class JwtTokenBuilderService(IConfiguration config) : IJwtTokenBuilder
{
    private readonly IConfiguration _config = config;

    public JwtServiceResult BuildJwtToken(ApplicationUserEntity entity)
    {
        var jwt = _config.GetSection("Jwt");
        var expiresMinutes = int.Parse(jwt["ExpiresMinutes"]!);
        var expiresAtUtc = DateTime.UtcNow.AddMinutes(expiresMinutes);
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, entity.Id),
            new Claim(JwtRegisteredClaimNames.Email, entity.Email ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, entity.Id),
            new Claim(ClaimTypes.Email, entity.Email ?? string.Empty),
            new Claim(ClaimTypes.Name, entity.UserName ?? entity.Email ?? string.Empty),
            new Claim("security_stamp", entity.SecurityStamp ?? string.Empty)
        };

        var token = new JwtSecurityToken(
            issuer: jwt["Issuer"],
            audience: jwt["Audience"],
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expiresAtUtc,
            signingCredentials: creds
        );

        return new JwtServiceResult
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiresAtUtc = expiresAtUtc
        };
    }
}
