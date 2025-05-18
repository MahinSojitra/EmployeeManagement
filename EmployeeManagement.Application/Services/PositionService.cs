using AutoMapper;
using EmployeeManagement.Application.DTOs.Position;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Application.Wrappers;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Interfaces;

namespace EmployeeManagement.Application.Services
{
    public class PositionService : IPositionService
    {
        private readonly IPositionRepository _repository;
        private readonly IMapper _mapper;

        public PositionService(IPositionRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<IEnumerable<PositionDto>>> GetAllAsync()
        {
            var data = await _repository.GetAllAsync();
            return ApiResponse<IEnumerable<PositionDto>>.SuccessResponse(_mapper.Map<IEnumerable<PositionDto>>(data));
        }

        public async Task<ApiResponse<PositionDto>> GetByIdAsync(Guid id)
        {
            var position = await _repository.GetByIdAsync(id);
            return position == null
                ? ApiResponse<PositionDto>.Fail("Position not found")
                : ApiResponse<PositionDto>.SuccessResponse(_mapper.Map<PositionDto>(position));
        }

        public async Task<ApiResponse<string>> AddAsync(CreatePositionDto dto)
        {
            var entity = _mapper.Map<Position>(dto);
            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();
            return ApiResponse<string>.SuccessResponse("Position created");
        }

        public async Task<ApiResponse<string>> UpdateAsync(UpdatePositionDto dto)
        {
            var existing = await _repository.GetByIdAsync(dto.Id);
            if (existing == null)
                return ApiResponse<string>.Fail("Position not found");

            _mapper.Map(dto, existing);
            var updated = await _repository.UpdateAsync(existing);
            if (!updated) return ApiResponse<string>.Fail("Update failed");

            await _repository.SaveChangesAsync();
            return ApiResponse<string>.SuccessResponse("Position updated");
        }

        public async Task<ApiResponse<string>> DeleteAsync(Guid id)
        {
            var deleted = await _repository.DeleteAsync(id);
            if (!deleted) return ApiResponse<string>.Fail("Position not found");

            await _repository.SaveChangesAsync();
            return ApiResponse<string>.SuccessResponse("Position deleted");
        }
    }
}