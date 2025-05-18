using EmployeeManagement.Application.DTOs.Employee;
using EmployeeManagement.Application.Wrappers;

namespace EmployeeManagement.Application.Interfaces
{
    public interface IEmployeeService
    {
        Task<ApiResponse<IEnumerable<EmployeeDto>>> GetAllAsync();
        Task<ApiResponse<EmployeeDto>> GetByIdAsync(Guid id);
        Task<ApiResponse<string>> CreateAsync(EmployeeDto dto);
        Task<ApiResponse<string>> UpdateAsync(UpdateEmployeeDto dto);
        Task<ApiResponse<string>> DeleteAsync(Guid id);
    }
}
