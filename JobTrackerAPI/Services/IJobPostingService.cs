using JobTrackerAPI.DTOs;

public interface IJobPostingService
{
    Task<List<JobPostingResponseDto>> GetAllAsync();
    Task<JobPostingResponseDto?> GetByIdAsync(int id);
    Task<JobPostingResponseDto> CreateAsync(JobPostingCreateDto dto);

    Task<JobPostingResponseDto?> UpdateAsync(int id, JobPostingCreateDto dto);
    Task<bool> DeleteAsync(int id);
}
