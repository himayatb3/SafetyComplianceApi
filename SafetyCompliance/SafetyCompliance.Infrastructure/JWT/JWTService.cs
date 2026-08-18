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
        private readonly IConfiguration configutaion;

        public JWTService(IConfiguration configutaion)
        {
            this.configutaion = configutaion;
        }

        public string GenerateTokenAsync(User user)
        {
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var key =new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configutaion["JWT:SecretKey"]));
            var securityToken = tokenHandler.CreateToken(new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new List<Claim>
                {
                    new Claim(AppClaimTypes.AppClaimTypes.UserId, user.Id.ToString()),
                    new Claim(AppClaimTypes.AppClaimTypes.UserName, user.UserName),
                    new Claim(AppClaimTypes.AppClaimTypes.Email, user.Email)

                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(key,SecurityAlgorithms.HmacSha256Signature)
            });
            return tokenHandler.WriteToken(securityToken);
        }
    }
}
