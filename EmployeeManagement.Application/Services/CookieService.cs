using EmployeeManagement.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace EmployeeManagement.Application.Services
{
    public class CookieService : ICookieService
    {
        public void SetTokenCookies(HttpResponse response, string accessToken, string refreshToken)
        {
            response.Cookies.Append("access", accessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(15)
            });

            response.Cookies.Append("refresh", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });
        }

        public void DeleteTokenCookies(HttpResponse response)
        {
            response.Cookies.Delete("access");
            response.Cookies.Delete("refresh");
        }
    }
}
