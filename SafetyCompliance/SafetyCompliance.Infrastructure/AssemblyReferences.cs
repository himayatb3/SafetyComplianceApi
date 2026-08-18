using Microsoft.Extensions.DependencyInjection;
using SafetyCompliance.Application.Abstraction.IJWT;
using SafetyCompliance.Infrastructure.JWT;

namespace SafetyCompliance.Infrastructure
{
    public static class AssemblyReferences
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services)
        {
            services.AddScoped<IJWTService, JWTService>();
      
            return services;
        }
    }
}
