using JobTrackerAPI.DTOs;
using JobTrackerAPI.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/admin")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary([FromQuery] string role)
    {
        if (role != "Admin")
        {
            return Unauthorized(ApiResponse<string>.ErrorResponse("Access denied: Admins only"));
        }

        var summary = await _adminService.GetSummaryAsync();
        return Ok(ApiResponse<AdminSummaryDto>.SuccessResponse("Summary fetched", summary));
    }

    [HttpGet("all-applications")]
    public async Task<IActionResult> GetAllApplications([FromQuery] string role)
    {
        if (role != "Admin")
        {
            return Unauthorized(ApiResponse<string>.ErrorResponse("Access denied: Admins only"));
        }

        var apps = await _adminService.GetAllApplicationsAsync();
        return Ok(ApiResponse<List<JobApplicationResponseDto>>.SuccessResponse("All applications fetched", apps));
    }
}
