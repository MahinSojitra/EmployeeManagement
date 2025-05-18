using AutoMapper;
using EmployeeManagement.Application.DTOs.Department;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Application.Wrappers;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Interfaces;

namespace EmployeeManagement.Application.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _repository;
        private readonly IMapper _mapper;

        public DepartmentService(IDepartmentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<IEnumerable<DepartmentDto>>> GetAllDepartmentsAsync()
        {
            var departments = await _repository.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<DepartmentDto>>(departments);
            return ApiResponse<IEnumerable<DepartmentDto>>.SuccessResponse(dtos);
        }

        public async Task<ApiResponse<DepartmentDto?>> GetDepartmentByIdAsync(Guid id)
        {
            var department = await _repository.GetByIdAsync(id);
            if (department == null)
                return ApiResponse<DepartmentDto?>.Fail("Department not found");

            var dto = _mapper.Map<DepartmentDto>(department);
            return ApiResponse<DepartmentDto?>.SuccessResponse(dto);
        }

        public async Task<ApiResponse<bool>> AddDepartmentAsync(CreateDepartmentDto departmentDto)
        {
            var department = _mapper.Map<Department>(departmentDto);
            await _repository.AddAsync(department);
            await _repository.SaveChangesAsync();
            return ApiResponse<bool>.SuccessResponse(true, "Department added");
        }

        public async Task<ApiResponse<bool>> UpdateDepartmentAsync(UpdateDepartmentDto departmentDto)
        {
            var existing = await _repository.GetByIdAsync(departmentDto.Id);
            if (existing == null)
                return ApiResponse<bool>.Fail("Department not found");

            _mapper.Map(departmentDto, existing);

            var updated = await _repository.UpdateAsync(existing);
            if (!updated)
                return ApiResponse<bool>.Fail("Failed to update department");

            await _repository.SaveChangesAsync();
            return ApiResponse<bool>.SuccessResponse(true, "Department updated");
        }

        public async Task<ApiResponse<bool>> DeleteDepartmentAsync(Guid id)
        {
            var deleted = await _repository.DeleteAsync(id);
            if (!deleted)
                return ApiResponse<bool>.Fail("Department not found");

            await _repository.SaveChangesAsync();
            return ApiResponse<bool>.SuccessResponse(true, "Department deleted");
        }

        public async Task<bool> DepartmentExistsAsync(Guid id)
        {
            return await _repository.ExistsAsync(id);
        }
    }
}