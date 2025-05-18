using EmployeeManagement.Application.DTOs.Auth;
using EmployeeManagement.Application.DTOs.Token;

namespace EmployeeManagement.Application.Interfaces
{
    public interface IAuthService
    {
        Task<SignInResponseDto> LoginAsync(SignInRequestDto request);
        Task<CustomTokenValidationResult> VerifyTokenAsync(string accessToken);
        Task<SignInResponseDto> RefreshTokenAsync(string refreshToken);
        Task<UserProfileDto?> GetUserProfileByEmailAsync(string email);
    }

}
