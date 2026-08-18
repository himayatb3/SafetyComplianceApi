using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SafetyCompliance.Persistence.Data;

namespace SafetyCompliance.Persistence
{
    public static class AssemblyReferences
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SafetyComplianceDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString(nameof(SafetyComplianceDbContext))));
            return services;
        }
    }
}
