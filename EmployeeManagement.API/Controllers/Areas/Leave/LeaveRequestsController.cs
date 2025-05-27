using EmployeeManagement.Application.DTOs.Leave;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers.Areas.Leave
{
    [ApiController]
    [Route("api/[area]")]
    [Area("leave")]
    [Authorize]
    public class LeaveRequestsController : ControllerBase
    {
        private readonly ILeaveRequestService _leaveService;

        public LeaveRequestsController(ILeaveRequestService leaveService)
        {
            _leaveService = leaveService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<LeaveRequestDto>>>> GetAll()
        {
            var response = await _leaveService.GetAllAsync();
            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ApiResponse<LeaveRequestDto>>> GetById(Guid id)
        {
            var response = await _leaveService.GetByIdAsync(id);
            if (!response.Success)
                return NotFound(response);

            return Ok(response);
        }

        [HttpGet("employee/{email}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<LeaveRequestDto>>>> GetByEmployeeEmail(string email)
        {
            var response = await _leaveService.GetByEmployeeEmailAsync(email);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<LeaveRequestDto>>> Create([FromBody] CreateLeaveRequestDto dto)
        {
            var response = await _leaveService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = response.Data?.Id }, response);
        }

        [HttpPut("approve/{id:guid}")]
        public async Task<ActionResult<ApiResponse<bool>>> Approve(Guid id)
        {
            var response = await _leaveService.UpdateStatusAsync(id, "Approved");
            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPut("reject/{id:guid}")]
        public async Task<ActionResult<ApiResponse<bool>>> Reject(Guid id)
        {
            var response = await _leaveService.UpdateStatusAsync(id, "Rejected");
            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(Guid id)
        {
            var response = await _leaveService.DeleteAsync(id);
            if (!response.Success)
                return NotFound(response);

            return Ok(response);
        }
    }
}
