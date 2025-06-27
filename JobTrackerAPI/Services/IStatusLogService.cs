using JobTrackerAPI.DTOs;

public interface IStatusLogService
{
    Task<List<StatusLogResponseDto>?> GetByApplicationIdAsync(int applicationId, int userId, string role);

}
