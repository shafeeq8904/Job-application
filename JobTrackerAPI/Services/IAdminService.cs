using JobTrackerAPI.DTOs;

public interface IAdminService
{
    Task<AdminSummaryDto> GetSummaryAsync();

    Task<List<JobApplicationResponseDto>> GetAllApplicationsAsync();


}
