using EmployeeManagement.Domain.Enums;

namespace EmployeeManagement.Domain.Entities
{
    public class LeaveRequest
    {
        public Guid Id { get; set; }

        public Guid EmployeeId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Reason { get; set; }

        public LeaveStatus Status { get; set; } = LeaveStatus.Pending;

        public ApplicationUser Employee { get; set; } = null!;
    }
}
