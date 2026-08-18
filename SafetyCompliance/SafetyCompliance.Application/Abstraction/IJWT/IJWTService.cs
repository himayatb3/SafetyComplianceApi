using SafetyCompliance.Domain.Entity;

namespace SafetyCompliance.Application.Abstraction.IJWT
{
    public interface IJWTService
    {
        string GenerateTokenAsync(User user);
    }
}
