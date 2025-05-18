using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Enums;

namespace EmployeeManagement.Application.DTOs.Employee
{
    public class EmployeeDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public DateTime HireDate { get; set; }
        public EmployeeType EmployeeType { get; set; }

        public Guid DepartmentId { get; set; }   // just IDs
        public Guid PositionId { get; set; }     // just IDs

        public string? DepartmentName { get; set; }
        public string? PositionTitle { get; set; }

        public Address? Address { get; set; }
    }

}