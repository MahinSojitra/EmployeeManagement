using AutoMapper;
using EmployeeManagement.Application.DTOs.Employee;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Application.Wrappers;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Interfaces;

namespace EmployeeManagement.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;
        private readonly IMapper _mapper;

        public EmployeeService(IEmployeeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<IEnumerable<EmployeeDto>>> GetAllAsync()
        {
            var users = await _repository.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<EmployeeDto>>(users);
            return ApiResponse<IEnumerable<EmployeeDto>>.SuccessResponse(dtos);
        }

        public async Task<ApiResponse<EmployeeDto>> GetByIdAsync(Guid id)
        {
            var user = await _repository.GetByIdAsync(id);
            if (user == null)
                return ApiResponse<EmployeeDto>.Fail("Employee not found");

            var dto = _mapper.Map<EmployeeDto>(user);
            return ApiResponse<EmployeeDto>.SuccessResponse(dto);
        }

        public async Task<ApiResponse<string>> CreateAsync(EmployeeDto dto)
        {
            try
            {
                var user = _mapper.Map<ApplicationUser>(dto);

                var password = GeneratePassword(dto);

                var created = await _repository.CreateAsync(user, password);

                return created
                    ? ApiResponse<string>.SuccessResponse("Employee created.")
                    : ApiResponse<string>.Fail("Failed to create employee");
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.Fail($"Internal error: {ex.Message}");
            }
        }

        private string GeneratePassword(EmployeeDto dto)
        {
            var firstName = dto.FirstName?.Trim() ?? "";
            var lastName = dto.LastName?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            {
                throw new ArgumentException("First name or last name is missing.");
            }

            return $"{firstName}{lastName}@123";
        }

        public async Task<ApiResponse<string>> UpdateAsync(UpdateEmployeeDto dto)
        {
            try
            {
                var existingUser = await _repository.GetByIdAsync(dto.Id);

                if (existingUser == null)
                    return ApiResponse<string>.Fail("Employee not found");

                _mapper.Map(dto, existingUser);
                
                var updated = await _repository.UpdateAsync(existingUser);

                return updated
                    ? ApiResponse<string>.SuccessResponse("Employee updated.")
                    : ApiResponse<string>.Fail("Update failed");
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.Fail($"Internal error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<string>> DeleteAsync(Guid id)
        {
            try
            {
                var deleted = await _repository.DeleteAsync(id);
                return deleted
                    ? ApiResponse<string>.SuccessResponse("Employee deleted.")
                    : ApiResponse<string>.Fail("Employee not found");
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.Fail($"Internal error: {ex.Message}");
            }
        }
    }
}