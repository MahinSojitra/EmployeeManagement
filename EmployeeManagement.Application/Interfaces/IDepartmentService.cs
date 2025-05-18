using EmployeeManagement.Application.DTOs.Department;
using EmployeeManagement.Application.Wrappers;

namespace EmployeeManagement.Application.Interfaces
{
    public interface IDepartmentService
    {
        Task<ApiResponse<IEnumerable<DepartmentDto>>> GetAllDepartmentsAsync();
        Task<ApiResponse<DepartmentDto?>> GetDepartmentByIdAsync(Guid id);
        Task<ApiResponse<bool>> AddDepartmentAsync(CreateDepartmentDto departmentDto);
        Task<ApiResponse<bool>> UpdateDepartmentAsync(UpdateDepartmentDto departmentDto);
        Task<ApiResponse<bool>> DeleteDepartmentAsync(Guid id);
        Task<bool> DepartmentExistsAsync(Guid id);
    }
}