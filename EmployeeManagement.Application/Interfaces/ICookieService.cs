using Microsoft.AspNetCore.Http;

namespace EmployeeManagement.Application.Interfaces
{
    public interface ICookieService
    {
        void SetTokenCookies(HttpResponse response, string accessToken, string refreshToken);
        void DeleteTokenCookies(HttpResponse response);
    }
}