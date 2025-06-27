using JobTrackerAPI.DTOs;

public interface IJobApplicationService
{
    Task<JobApplicationResponseDto> CreateApplicationAsync(JobApplicationCreateDto dto);
    Task<List<JobApplicationResponseDto>> GetApplicationsAsync(int userId, string role);
    Task<JobApplicationResponseDto?> GetByIdAsync(int id);
    Task<JobApplicationResponseDto?> UpdateAsync(int id, JobApplicationUpdateDto dto);
    Task<bool> DeleteAsync(int id, int userId);

}
