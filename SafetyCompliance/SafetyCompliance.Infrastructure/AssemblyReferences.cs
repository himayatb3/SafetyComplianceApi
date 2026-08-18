using Microsoft.Extensions.DependencyInjection;

namespace SafetyCompliance.Infrastructure
{
    public static class AssemblyReferences
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services)
        {
            // Add your infrastructure-related services here
            // For example, you can add logging, caching, etc.
            return services;
        }
    }
}
