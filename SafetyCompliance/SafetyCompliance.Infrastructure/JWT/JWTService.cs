using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SafetyCompliance.Application.Abstraction.IJWT;
using SafetyCompliance.Domain.Entity;
using System.Security.Claims;
using System.Text;

namespace SafetyCompliance.Infrastructure.JWT
{
    public class JWTService : IJWTService
    {
        private readonly IConfiguration configuration;

        public JWTService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string GenerateTokenAsync(User user)
        {
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var secretKey = configuration["JWT:Key"] ??
                throw new InvalidOperationException("JWT key is not configured.");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var securityToken = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new List<Claim>
                {
                    new Claim(AppClaimTypes.AppClaimTypes.UserId, user.Id.ToString()),
                    new Claim(AppClaimTypes.AppClaimTypes.UserName, user.UserName),
                    new Claim(AppClaimTypes.AppClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            });

            return tokenHandler.WriteToken(securityToken);
        }
    }
}
