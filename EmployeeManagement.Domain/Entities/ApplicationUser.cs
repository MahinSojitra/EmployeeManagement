using EmployeeManagement.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace EmployeeManagement.Domain.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;

        public DateTime DateOfBirth { get; set; }
        public DateTime HireDate { get; set; }
        public EmployeeType EmployeeType { get; set; }

        public Guid? DepartmentId { get; set; }
        public Department? Department { get; set; }

        public Address? Address { get; set; }   

        public ICollection<LeaveRequest> LeaveRequests { get; set; }
        public Guid? PositionId { get; set; }
        public Position? Position { get; set; }
    }
}