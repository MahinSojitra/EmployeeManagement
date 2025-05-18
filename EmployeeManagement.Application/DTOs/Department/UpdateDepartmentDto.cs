namespace EmployeeManagement.Application.DTOs.Department
{
    public class UpdateDepartmentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
