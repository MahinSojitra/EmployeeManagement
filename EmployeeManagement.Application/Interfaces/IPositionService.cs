using EmployeeManagement.Application.DTOs.Position;
using EmployeeManagement.Application.Wrappers;

namespace EmployeeManagement.Application.Interfaces
{
    public interface IPositionService
    {
        Task<ApiResponse<IEnumerable<PositionDto>>> GetAllAsync();
        Task<ApiResponse<PositionDto>> GetByIdAsync(Guid id);
        Task<ApiResponse<string>> AddAsync(CreatePositionDto dto);
        Task<ApiResponse<string>> UpdateAsync(UpdatePositionDto dto);
        Task<ApiResponse<string>> DeleteAsync(Guid id);
    }
}
