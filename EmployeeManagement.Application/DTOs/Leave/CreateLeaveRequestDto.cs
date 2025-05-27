namespace EmployeeManagement.Application.DTOs.Leave
{
    public class CreateLeaveRequestDto
    {
        public string EmployeeEmail { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
    }
}
