using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Application.DTOs.Auth
{
    public class SignInRequestDto
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please provide a valid email.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}
