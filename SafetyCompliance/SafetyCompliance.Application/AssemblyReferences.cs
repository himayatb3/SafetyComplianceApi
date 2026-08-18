using Microsoft.Extensions.DependencyInjection;

namespace SafetyCompliance.Application
{
    public static class AssemblyReferences
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Add your application-related services here
            // For example, you can add MediatR, AutoMapper, etc.
            return services;
        }
    }
}
