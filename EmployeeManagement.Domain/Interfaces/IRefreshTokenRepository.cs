using EmployeeManagement.Domain.Entities;

namespace EmployeeManagement.Domain.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task SaveAsync(RefreshToken refreshToken);
        Task<RefreshToken?> GetByTokenAsync(string token);
        Task<RefreshToken?> GetByIdAsync(Guid id);
        Task UpdateAsync(RefreshToken refreshToken);
        Task DeleteAsync(Guid id);
    }
}
