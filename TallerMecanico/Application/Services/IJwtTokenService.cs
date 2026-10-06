using TallerMecanico.Domain.Agregates;

namespace TallerMecanico.Application.Services
{
    public interface IJwtTokenService
    {
        string GenerateToken(Employee employee);
    }
}
