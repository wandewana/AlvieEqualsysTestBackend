using AlvieEqualsysTestBackend.Models;
using AlvieEqualsysTestBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace AlvieEqualsysTestBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobPositionsController : ControllerBase
    {
        private readonly IJobPositionService _jobService;

        public JobPositionsController(IJobPositionService jobService)
        {
            _jobService = jobService;
        }
        
        [HttpGet("employee/{employeeId}")]
        public async Task<ActionResult<List<JobPosition>>> GetByEmployee(int employeeId)
        {
            var jobs = await _jobService.GetByEmployeeIdAsync(employeeId);
            return Ok(jobs);
        }

        [HttpPost("employee/{employeeId}")]
        public async Task<ActionResult<JobPosition>> AddToEmployee(int employeeId, [FromBody] JobPosition job)
        {
            try
            {
                var result = await _jobService.AddToEmployeeAsync(employeeId, job);
                return CreatedAtAction(nameof(GetByEmployee), new { employeeId = employeeId }, result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<JobPosition>> Update(int id, [FromBody] JobPosition job)
        {
            try
            {
                var result = await _jobService.UpdateAsync(id, job);
                if (result == null) return NotFound();
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _jobService.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
