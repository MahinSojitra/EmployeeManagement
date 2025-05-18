using AutoMapper;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public EmployeeRepository(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllAsync()
        {
            return await _userManager.Users
                .Include(u => u.Department)
                .Include(u => u.Position)
                .Include(u => u.Address)
                .ToListAsync();
        }
            
        public async Task<ApplicationUser?> GetByIdAsync(Guid id)
        {
            return await _userManager.Users
                .Include(u => u.Department)
                .Include(u => u.Position)
                .Include(u => u.Address)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<bool> CreateAsync(ApplicationUser user, string password)
        {
            // Create the user
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                return false;

            // Add user to "Employee" role
            var roleAssignResult = await _userManager.AddToRoleAsync(user, "Employee");

            return roleAssignResult.Succeeded;
        }

        public async Task<bool> UpdateAsync(ApplicationUser user)
        {
            var existingUser = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            if (existingUser == null) return false;

            _mapper.Map(user, existingUser);

            var result = await _userManager.UpdateAsync(existingUser);
            return result.Succeeded;
        }

        public async Task<bool> DeleteAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return false;

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }
    }
}