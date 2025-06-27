using JobTrackerAPI.DTOs;
using JobTrackerAPI.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class JobApplicationsController : ControllerBase
{
    private readonly IJobApplicationService _applicationService;

    public JobApplicationsController(IJobApplicationService applicationService)
    {
        _applicationService = applicationService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _applicationService.GetByIdAsync(id);

        if (result == null)
        {
            return NotFound(ApiResponse<JobApplicationResponseDto>.ErrorResponse("Application not found"));
        }

        return Ok(ApiResponse<JobApplicationResponseDto>.SuccessResponse("Application fetched", result));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] JobApplicationCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
            );

            return BadRequest(ApiResponse<JobApplicationResponseDto>.ErrorResponse("Validation failed", errors));
        }

        try
        {
            var result = await _applicationService.CreateApplicationAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id },
                ApiResponse<JobApplicationResponseDto>.SuccessResponse("Application created", result));
        }
        catch (Exception ex)
        {
            return NotFound(ApiResponse<JobApplicationResponseDto>.ErrorResponse("Failed to create application", new
            {
                general = new[] { ex.Message }
            }));
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetApplications([FromQuery] int userId, [FromQuery] string role)
    {

        if (string.IsNullOrWhiteSpace(role))
        {
            return BadRequest(ApiResponse<List<JobApplicationResponseDto>>.ErrorResponse("Role is required"));
        }

        if (role != "Admin" && userId <= 0)
        {
            return BadRequest(ApiResponse<List<JobApplicationResponseDto>>.ErrorResponse("Invalid userId for non-admin role"));
        }

        var apps = await _applicationService.GetApplicationsAsync(userId, role);
        if (apps == null || !apps.Any())
        {
            return NotFound(ApiResponse<List<JobApplicationResponseDto>>.ErrorResponse("No applications found"));
        }

        return Ok(ApiResponse<List<JobApplicationResponseDto>>.SuccessResponse("Applications fetched", apps));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] JobApplicationUpdateDto dto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
            );

            return BadRequest(ApiResponse<JobApplicationResponseDto>.ErrorResponse("Validation failed", errors));
        }

        var result = await _applicationService.UpdateAsync(id, dto);

        if (result == null)
        {
            return NotFound(ApiResponse<JobApplicationResponseDto>.ErrorResponse("Application not found"));
        }

        return Ok(ApiResponse<JobApplicationResponseDto>.SuccessResponse("Application updated", result));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, [FromQuery] int userId)
    {
        if (userId <= 0)
        {
            return BadRequest(ApiResponse<string>.ErrorResponse("UserId is Invalid"));
        }

        var success = await _applicationService.DeleteAsync(id, userId);

        if (!success)
        {
            return NotFound(ApiResponse<string>.ErrorResponse("Application not found or access denied"));
        }

        return Ok(ApiResponse<string>.SuccessResponse("Application deleted", "Deleted successfully"));
    }


}
