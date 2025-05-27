using EmployeeManagement.Application.DTOs.Leave;
using EmployeeManagement.Application.Wrappers;

namespace EmployeeManagement.Application.Interfaces
{
    public interface ILeaveRequestService
    {
        Task<ApiResponse<IEnumerable<LeaveRequestDto>>> GetAllAsync();
        Task<ApiResponse<LeaveRequestDto>> GetByIdAsync(Guid id);
        Task<ApiResponse<IEnumerable<LeaveRequestDto>>> GetByEmployeeEmailAsync(string email);
        Task<ApiResponse<LeaveRequestDto>> CreateAsync(CreateLeaveRequestDto dto);
        Task<ApiResponse<bool>> UpdateStatusAsync(Guid id, string status);
        Task<ApiResponse<bool>> DeleteAsync(Guid id);
    }
}
