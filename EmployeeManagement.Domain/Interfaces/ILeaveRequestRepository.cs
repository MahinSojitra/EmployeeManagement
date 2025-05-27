using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Enums;

namespace EmployeeManagement.Domain.Interfaces
{
    public interface ILeaveRequestRepository
    {
        Task<IEnumerable<LeaveRequest>> GetAllAsync();
        Task<LeaveRequest?> GetByIdAsync(Guid id);
        Task<IEnumerable<LeaveRequest>> GetLeaveRequestsByEmailAsync(string email);
        Task AddAsync(LeaveRequest leaveRequest, string email);
        Task<bool> UpdateStatusAsync(Guid leaveRequestId, LeaveStatus status);
        void Delete(LeaveRequest leaveRequest);
        Task<bool> SaveChangesAsync();
    }
}
