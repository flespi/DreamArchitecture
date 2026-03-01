using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace CleanArchitecture.Graph.FunctionalTests;

public static class JwtHelper
{
    public static SymmetricSecurityKey SigningKey { get; } = GenerateSigningKey();

    private static SymmetricSecurityKey GenerateSigningKey()
    {
        var secret = Guid.NewGuid().ToString();
        var key = Encoding.UTF8.GetBytes(secret);
        return new SymmetricSecurityKey(key);
    }

    public static string GenerateJwtToken(IEnumerable<Claim> claims)
    {
        var credentials = new SigningCredentials(SigningKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: credentials
        );

        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.WriteToken(token);

        return jwtToken;
    }
}
