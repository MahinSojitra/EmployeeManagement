using EmployeeManagement.Application.DTOs.Token;
using EmployeeManagement.Domain.Entities;

namespace EmployeeManagement.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(ApplicationUser user, IList<string>? roles);
        string GenerateRefreshToken(ApplicationUser user);
        Task<CustomTokenValidationResult> ValidateAccessTokenAsync(string token);
    }

}
