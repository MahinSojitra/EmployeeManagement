namespace EmployeeManagement.Application.DTOs.Auth
{
    public class SignInResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public UserResponse user { get; set; }
        public IEnumerable<string> Errors { get; set; } = Array.Empty<string>();
    }
}
