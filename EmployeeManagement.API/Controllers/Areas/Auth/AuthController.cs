using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Application.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/[area]")]
[Area("auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ICookieService _cookieService;

    public AuthController(IAuthService authService, ICookieService cookieService)
    {
        _authService = authService;
        _cookieService = cookieService;
    }

    //[HttpPost("signup")]
    //public async Task<IActionResult> Register([FromBody] SignUpRequestDto request)
    //{
    //    if (!ModelState.IsValid)
    //        return BadRequest(new SignUpResponseDto
    //        {
    //            Success = false,
    //            Message = "Validation errors.",
    //            Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).Distinct()
    //        });

    //    var result = await _authService.RegisterAsync(request);

    //    if (!result.Success)
    //        return Conflict(result);

    //    return Ok(result);
    //}

    [HttpPost("signin")]
    public async Task<IActionResult> Login([FromBody] SignInRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new SignInResponseDto
            {
                Success = false,
                Message = "Validation failed.",
                Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).Distinct()
            });
        }

        var result = await _authService.LoginAsync(request);

        if (!result.Success)
            return Unauthorized(result);

        // Set HttpOnly secure cookies
        _cookieService.SetTokenCookies(Response, result.AccessToken!, result.RefreshToken!);

        return Ok(result);
    }

    [HttpPost("signout")]
    public IActionResult Logout()
    {
        _cookieService.DeleteTokenCookies(Response);

        return Ok(new
        {
            Success = true,
            Message = "Sign out successful."
        });
    }

    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrEmpty(email))
            return Unauthorized(new { Success = false, Message = "Email not found in token." });

        var profile = await _authService.GetUserProfileByEmailAsync(email);
        if (profile == null)
            return NotFound(new { Success = false, Message = "User not found." });

        return Ok(new
        {
            Success = true,
            Data = profile
        });
    }


    [HttpPost("verify-token")]
    public async Task<IActionResult> VerifyToken()
    {
        if (!Request.Headers.TryGetValue("Authorization", out var accessToken))
            return BadRequest(new { message = "Access token is missing in the Authorization header." });

        // Remove "Bearer " prefix if present
        var token = accessToken.ToString().Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase);

        var isValid = await _authService.VerifyTokenAsync(token);
        return Ok(new { isValid });
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken()
    {
        if (!Request.Headers.TryGetValue("X-Refresh-Token", out var refreshToken))
            return BadRequest(new { message = "Refresh token is missing in the X-Refresh-Token header." });

        var token = refreshToken.ToString();
        var response = await _authService.RefreshTokenAsync(token);

        if (!response.Success)
            return Unauthorized(response);

        return Ok(response);
    }
}