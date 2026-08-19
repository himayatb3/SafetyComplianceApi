using Microsoft.Extensions.DependencyInjection;
using SafetyCompliance.Application.Abstraction.IService;
using SafetyCompliance.Application.Services;

namespace SafetyCompliance.Application
{
    public static class AssemblyReferences
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            return services;
        }
    }
}
