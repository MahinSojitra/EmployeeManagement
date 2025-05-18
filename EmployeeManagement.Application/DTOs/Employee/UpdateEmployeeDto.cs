using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Enums;

namespace EmployeeManagement.Application.DTOs.Employee
{
    public class UpdateEmployeeDto
    {
        public Guid Id { get; set; }

        public string Email { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public DateTime HireDate { get; set; }

        public EmployeeType EmployeeType { get; set; }

        public Guid DepartmentId { get; set; }

        public Guid? PositionId { get; set; }

        public Address? Address { get; set; }
    }
}
