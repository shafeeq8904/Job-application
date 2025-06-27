using JobTrackerAPI.DTOs;
using JobTrackerAPI.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class StatusLogsController : ControllerBase
{
    private readonly IStatusLogService _logService;

    public StatusLogsController(IStatusLogService logService)
    {
        _logService = logService;
    }

    [HttpGet("{applicationId}")]
    public async Task<IActionResult> GetByApplicationId(int applicationId, [FromQuery] int userId, [FromQuery] string role)
    {
        var logs = await _logService.GetByApplicationIdAsync(applicationId, userId, role);

        if (logs == null)
        {
            return NotFound(ApiResponse<List<StatusLogResponseDto>>.ErrorResponse("Logs not found or access denied"));
        }

        return Ok(ApiResponse<List<StatusLogResponseDto>>.SuccessResponse("Status history fetched", logs));
    }
}
