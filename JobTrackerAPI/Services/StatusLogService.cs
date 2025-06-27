using JobTrackerAPI.DTOs;
using JobTrackerAPI.Models;
using JobTrackerAPI.Repositories;
using Microsoft.EntityFrameworkCore;

public class StatusLogService : IStatusLogService
{
    private readonly IRepository<StatusLog> _logRepo;
    private readonly IRepository<JobApplication> _applicationRepo;

    public StatusLogService(IRepository<StatusLog> logRepo, IRepository<JobApplication> applicationRepo)
    {
        _logRepo = logRepo;
        _applicationRepo = applicationRepo;
    }

    public async Task<List<StatusLogResponseDto>?> GetByApplicationIdAsync(int applicationId, int userId, string role)
    {
    var apps = await _applicationRepo.FindAsync(
        a => a.Id == applicationId,
        q => q.Include(a => a.User)
    );
    var app = apps.FirstOrDefault();

    if (app == null)
        return null;

    if (role != "Admin" && app.UserId != userId)
        return null;
    var logs = await _logRepo.FindAsync(l => l.ApplicationId == applicationId);

    return logs
        .OrderBy(l => l.Timestamp)
        .Select(l => new StatusLogResponseDto
        {
            NewStatus = l.NewStatus,
            Timestamp = l.Timestamp
        })
        .ToList();
    }
}
