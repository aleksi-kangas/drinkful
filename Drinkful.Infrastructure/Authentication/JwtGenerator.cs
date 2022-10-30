using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Drinkful.Application.Common.Interfaces.Authentication;
using Drinkful.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Drinkful.Infrastructure.Authentication;

public class JwtGenerator : IJwtGenerator {
  private readonly JwtSettings _jwtSettings;

  public JwtGenerator(IOptions<JwtSettings> jwtOptions) {
    _jwtSettings = jwtOptions.Value;
  }

  public string GenerateToken(User user) {
    var claims = new[] {
      new Claim(JwtRegisteredClaimNames.Email, user.Email),
      new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
      new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
    };
    var signingCredentials = new SigningCredentials(
      new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
      SecurityAlgorithms.HmacSha256Signature);
    var securityToken = new JwtSecurityToken(
      audience: _jwtSettings.Audience,
      claims: claims,
      expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
      issuer: _jwtSettings.Issuer,
      signingCredentials: signingCredentials);
    return new JwtSecurityTokenHandler().WriteToken(securityToken);
  }
}