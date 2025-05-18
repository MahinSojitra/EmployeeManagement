namespace EmployeeManagement.Application.DTOs.Auth
{
    public class UserProfileDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public DateTime HireDate { get; set; }
        public string EmployeeType { get; set; } = null!;
        public string? DepartmentName { get; set; }
        public string? PositionTitle { get; set; }
        public AddressDto? Address { get; set; }
    }
}
