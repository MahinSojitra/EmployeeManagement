using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Application.DTOs.Auth;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Interfaces;
using EmployeeManagement.Domain.Repositories;
using EmployeeManagement.Application.DTOs.Token;
using AutoMapper;

namespace EmployeeManagement.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IMapper _mapper;

        public AuthService(IUserRepository userRepository, ITokenService tokenService, IRefreshTokenRepository refreshTokenRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _refreshTokenRepository = refreshTokenRepository;
            _mapper = mapper;
        }

        public async Task<SignInResponseDto> LoginAsync(SignInRequestDto request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null || !await _userRepository.CheckPasswordAsync(user, request.Password))
            {
                return new SignInResponseDto
                {
                    Success = false,
                    Message = "Invalid email or password.",
                    Errors = Array.Empty<string>()
                };
            }

            var roles = await _userRepository.GetRolesAsync(user);

            var accessToken = _tokenService.GenerateAccessToken(user, roles);
            var refreshToken = _tokenService.GenerateRefreshToken(user);

            await _refreshTokenRepository.SaveAsync(new RefreshToken
            {
                Id = Guid.NewGuid(),
                Token = refreshToken,
                UserId = user.Id.ToString(),
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            });

            return new SignInResponseDto
            {
                Success = true,
                Message = "Signin successful.",
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                user = new UserResponse
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email!,
                    Roles = roles,
                },
                Errors = Array.Empty<string>()
            };
        }

        public async Task<UserProfileDto?> GetUserProfileByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            return user == null ? null : _mapper.Map<UserProfileDto>(user);
        }

        public async Task<CustomTokenValidationResult> VerifyTokenAsync(string accessToken)
        {
            return await _tokenService.ValidateAccessTokenAsync(accessToken);
        }

        public async Task<SignInResponseDto> RefreshTokenAsync(string refreshToken)
        {
            var storedToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken);
            if (storedToken == null || storedToken.IsRevoked || storedToken.ExpiryDate < DateTime.UtcNow)
            {
                return new SignInResponseDto
                {
                    Success = false,
                    Message = "Invalid or expired refresh token.",
                    Errors = new[] { "Invalid refresh token." }
                };
            }

            var user = await _userRepository.GetByIdAsync(storedToken.UserId);
            if (user == null)
            {
                return new SignInResponseDto
                {
                    Success = false,
                    Message = "User not found.",
                    Errors = new[] { "Associated user not found." }
                };
            }

            var roles = await _userRepository.GetRolesAsync(user);
            var newAccessToken = _tokenService.GenerateAccessToken(user, roles);
            var newRefreshToken = _tokenService.GenerateRefreshToken(user);

            // Revoke old token and save new one
            storedToken.IsRevoked = true;
            await _refreshTokenRepository.UpdateAsync(storedToken);

            await _refreshTokenRepository.SaveAsync(new RefreshToken
            {
                Id = Guid.NewGuid(),
                Token = newRefreshToken,
                UserId = user.Id.ToString(),
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            });

            return new SignInResponseDto
            {
                Success = true,
                Message = "Token refreshed",
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                user = new UserResponse
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email!,
                    Roles = roles,
                },
                Errors = Array.Empty<string>()
            };
        }
    }
}
