namespace EmployeeManagement.Domain.Entities
{
    public class LeaveRequest
    {
        public Guid Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; } // Pending, Approved, Rejected

        public Guid EmployeeId { get; set; }
        public ApplicationUser Employee { get; set; }
    }

}
