using JobTrackerAPI.DTOs;
using JobTrackerAPI.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/jobpostings")]
public class JobPostingController : ControllerBase
{
    private readonly IJobPostingService _service;

    public JobPostingController(IJobPostingService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var jobs = await _service.GetAllAsync();
        return Ok(ApiResponse<List<JobPostingResponseDto>>.SuccessResponse("Job postings fetched", jobs));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var job = await _service.GetByIdAsync(id);
        if (job == null)
            return NotFound(ApiResponse<JobPostingResponseDto>.ErrorResponse("Job posting not found"));

        return Ok(ApiResponse<JobPostingResponseDto>.SuccessResponse("Job posting found", job));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromQuery] string role, [FromBody] JobPostingCreateDto dto)
    {
        if (role != "Admin")
            return Unauthorized(ApiResponse<string>.ErrorResponse("Access denied: Admins only"));

        if (!ModelState.IsValid)
        {
            var errors = ModelState.ToDictionary(
                e => e.Key,
                e => e.Value!.Errors.Select(er => er.ErrorMessage).ToArray()
            );

            return BadRequest(ApiResponse<JobPostingResponseDto>.ErrorResponse("Validation failed", errors));
        }

        var job = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = job.Id }, ApiResponse<JobPostingResponseDto>.SuccessResponse("Job created", job));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromQuery] string role, [FromBody] JobPostingCreateDto dto)
    {
        if (role != "Admin")
            return Unauthorized(ApiResponse<string>.ErrorResponse("Access denied: Admins only"));

        if (!ModelState.IsValid)
        {
            var errors = ModelState.ToDictionary(
                e => e.Key,
                e => e.Value!.Errors.Select(er => er.ErrorMessage).ToArray()
            );

            return BadRequest(ApiResponse<JobPostingResponseDto>.ErrorResponse("Validation failed", errors));
        }

        var result = await _service.UpdateAsync(id, dto);
        if (result == null)
            return NotFound(ApiResponse<JobPostingResponseDto>.ErrorResponse("Job posting not found"));

        return Ok(ApiResponse<JobPostingResponseDto>.SuccessResponse("Job updated", result));
    }

    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, [FromQuery] string role)
    {
        if (role != "Admin")
            return Unauthorized(ApiResponse<string>.ErrorResponse("Access denied: Admins only"));

        var deleted = await _service.DeleteAsync(id);
        if (!deleted)
            return NotFound(ApiResponse<string>.ErrorResponse("Job posting not found"));

        return Ok(ApiResponse<string>.SuccessResponse("Job deleted", "Deleted successfully"));
    }

}
