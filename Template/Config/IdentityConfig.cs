using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core.DTO;
using Data.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Template.Config;

public class IdentityConfig
{
    private readonly IConfiguration _config;

    public IdentityConfig(IConfiguration config)
    {
        _config = config;
    }

    public TokenResponseDto GenerateToken(User user)
    {
        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username ?? ""),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:JwtAuthSecret"]!));
        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            expires: DateTime.UtcNow.AddMinutes(double.Parse(_config["Jwt:ExpiresInMinutes"]!)),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return new TokenResponseDto()
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiresIn = _config["Jwt:ExpiresInMinutes"]!
        };
    }
}