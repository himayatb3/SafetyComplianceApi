using Microsoft.Extensions.DependencyInjection;
using SafetyCompliance.Application.Abstraction.IJWT;
using SafetyCompliance.Application.Abstraction.ISecurity;
using SafetyCompliance.Infrastructure.JWT;
using SafetyCompliance.Infrastructure.Security;

namespace SafetyCompliance.Infrastructure
{
    public static class AssemblyReferences
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services)
        {
            services.AddScoped<IJWTService, JWTService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();

            return services;
        }
    }
}
