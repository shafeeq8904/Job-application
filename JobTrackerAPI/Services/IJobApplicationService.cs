using JobTrackerAPI.DTOs;

public interface IJobApplicationService
{
    Task<JobApplicationResponseDto> CreateApplicationAsync(JobApplicationCreateDto dto);
    Task<List<JobApplicationResponseDto>> GetApplicationsAsync(int userId, string role, string? status = null, string? companyName = null);
    Task<JobApplicationResponseDto?> GetByIdAsync(int id, int userId, string role);
    Task<JobApplicationResponseDto?> UpdateAsync(int id, JobApplicationUpdateDto dto);
    Task<bool> DeleteAsync(int id, int userId ,string role);

}
