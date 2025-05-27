using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Enums;
using EmployeeManagement.Domain.Interfaces;
using EmployeeManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Infrastructure.Repositories
{
    public class LeaveRequestRepository : ILeaveRequestRepository
    {
        private readonly AppDbContext _context;

        public LeaveRequestRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LeaveRequest>> GetAllAsync()
        {
            return await _context.LeaveRequests
                .Include(leaveRequest => leaveRequest.Employee)
                .ToListAsync();
        }

        public async Task<LeaveRequest?> GetByIdAsync(Guid id)
        {
            return await _context.LeaveRequests
                .Include(leaveRequest => leaveRequest.Employee)
                .FirstOrDefaultAsync(leaveRequest => leaveRequest.Id == id);
        }

        public async Task<IEnumerable<LeaveRequest>> GetLeaveRequestsByEmailAsync(string email)
        {
            return await _context.LeaveRequests
                .Join(
                    _context.Users,
                    leave => leave.EmployeeId,
                    user => user.Id,
                    (leave, user) => new { Leave = leave, User = user }
                )
                .Where(joined => joined.User.Email == email)
                .Select(joined => joined.Leave)
                .ToListAsync();
        }

        public async Task AddAsync(LeaveRequest leaveRequest, string email)
        {
            var employee = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            if (employee == null)
                throw new Exception("Employee not found with the provided email.");

            leaveRequest.EmployeeId = employee.Id;

            _context.LeaveRequests.Add(leaveRequest);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateStatusAsync(Guid leaveRequestId, LeaveStatus status)
        {
            var leaveRequest = await _context.LeaveRequests.FindAsync(leaveRequestId);
            if (leaveRequest == null)
                return false;

            leaveRequest.Status = status;
            _context.LeaveRequests.Update(leaveRequest);
            return await _context.SaveChangesAsync() > 0;
        }

        public void Delete(LeaveRequest leaveRequest)
        {
            _context.LeaveRequests.Remove(leaveRequest);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
