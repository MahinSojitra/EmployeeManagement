using EmployeeManagement.Application.DTOs.Employee;
using EmployeeManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers.Areas.Employee
{
    [ApiController]
    [Route("api/[area]")]
    [Area("employee")]
    //[Authorize(Roles = "Admin")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        // GET: api/employee
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _employeeService.GetAllAsync();
            return result.Success
                ? Ok(result)
                : StatusCode(500, result);
        }

        // GET: api/employee/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _employeeService.GetByIdAsync(id);
            return result.Success
                ? Ok(result)
                : NotFound(result);
        }

        // POST: api/employee
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EmployeeDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            var result = await _employeeService.CreateAsync(request);
            return result.Success
                ? Ok(result)
                : BadRequest(result);
        }

        // PUT: api/employee
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateEmployeeDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            var result = await _employeeService.UpdateAsync(dto);
            return result.Success
                ? Ok(result)
                : BadRequest(result);
        }

        // DELETE: api/employee/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _employeeService.DeleteAsync(id);
            return result.Success
                ? Ok(result)
                : NotFound(result);
        }
    }
}
