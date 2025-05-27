using AutoMapper;
using EmployeeManagement.Application.DTOs.Leave;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Application.Wrappers;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Interfaces;

namespace EmployeeManagement.Application.Services
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly ILeaveRequestRepository _repository;
        private readonly IMapper _mapper;

        public LeaveRequestService(ILeaveRequestRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<IEnumerable<LeaveRequestDto>>> GetAllAsync()
        {
            var leaveRequests = await _repository.GetAllAsync();
            var dto = _mapper.Map<IEnumerable<LeaveRequestDto>>(leaveRequests);
            return ApiResponse<IEnumerable<LeaveRequestDto>>.SuccessResponse(dto);
        }

        public async Task<ApiResponse<LeaveRequestDto>> GetByIdAsync(Guid id)
        {
            var leaveRequest = await _repository.GetByIdAsync(id);
            if (leaveRequest == null)
                return ApiResponse<LeaveRequestDto>.Fail("Leave request not found.");

            return ApiResponse<LeaveRequestDto>.SuccessResponse(_mapper.Map<LeaveRequestDto>(leaveRequest));
        }

        public async Task<ApiResponse<IEnumerable<LeaveRequestDto>>> GetByEmployeeEmailAsync(string email)
        {
            var leaves = await _repository.GetLeaveRequestsByEmailAsync(email);
            var dto = _mapper.Map<IEnumerable<LeaveRequestDto>>(leaves);
            return ApiResponse<IEnumerable<LeaveRequestDto>>.SuccessResponse(dto);
        }

        public async Task<ApiResponse<LeaveRequestDto>> CreateAsync(CreateLeaveRequestDto dto)
        {
            var leaveRequest = _mapper.Map<LeaveRequest>(dto);
            leaveRequest.Status = Domain.Enums.LeaveStatus.Pending;

            await _repository.AddAsync(leaveRequest, dto.EmployeeEmail);
            await _repository.SaveChangesAsync();

            return ApiResponse<LeaveRequestDto>.SuccessResponse(_mapper.Map<LeaveRequestDto>(leaveRequest), "Leave request submitted.");
        }

        public async Task<ApiResponse<bool>> UpdateStatusAsync(Guid id, string status)
        {
            if (!Enum.TryParse<Domain.Enums.LeaveStatus>(status, true, out var parsedStatus))
                return ApiResponse<bool>.Fail("Invalid status value. Allowed values: Pending, Approved, Rejected.");

            var leave = await _repository.GetByIdAsync(id);
            if (leave == null)
                return ApiResponse<bool>.Fail("Leave request not found.");

            var success = await _repository.UpdateStatusAsync(id, parsedStatus);
            return success
                ? ApiResponse<bool>.SuccessResponse(true, $"Leave request {parsedStatus.ToString().ToLower()}.")
                : ApiResponse<bool>.Fail("Failed to update leave request status.");
        }

        public async Task<ApiResponse<bool>> DeleteAsync(Guid id)
        {
            var leave = await _repository.GetByIdAsync(id);
            if (leave == null)
                return ApiResponse<bool>.Fail("Leave request not found.");

            _repository.Delete(leave);
            var success = await _repository.SaveChangesAsync();

            return success
                ? ApiResponse<bool>.SuccessResponse(true, "Leave request deleted.")
                : ApiResponse<bool>.Fail("Failed to delete leave request.");
        }
    }
}
