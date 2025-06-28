using JobTrackerAPI.Data;
using JobTrackerAPI.DTOs;
using Microsoft.EntityFrameworkCore;

public class AdminService : IAdminService
{
    private readonly AppDbContext _context;

    public AdminService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<AdminSummaryDto> GetSummaryAsync()
    {
        var total = await _context.JobApplications.CountAsync();

        var statusCounts = await _context.JobApplications
            .GroupBy(a => a.Status)
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .ToDictionaryAsync(g => g.Status, g => g.Count);

        var perUser = await _context.Users
            .Where(u => u.Role == "User")
            .Select(u => new UserApplicationCountDto
            {
                UserId = u.Id,
                UserName = u.Name,
                ApplicationCount = u.Applications.Count
            })
            .ToListAsync();

        return new AdminSummaryDto
        {
            TotalApplications = total,
            StatusCounts = statusCounts,
            ApplicationsPerUser = perUser
        };
    }

    public async Task<List<JobApplicationResponseDto>> GetAllApplicationsAsync()
    {
        var apps = await _context.JobApplications
        .Include(a => a.User)
        .Include(a => a.JobPosting)
        .OrderByDescending(a => a.ApplicationDate)
        .ToListAsync();

        return apps.Select(app => new JobApplicationResponseDto
        {
            Id = app.Id,
            JobPostingId = app.JobPostingId,
            JobTitle = app.JobPosting.JobTitle,
            CompanyName = app.JobPosting.CompanyName,
            Location = app.JobPosting.Location,
            ApplicationDate = app.ApplicationDate,
            Status = app.Status,
            Notes = app.Notes,
            UserId = app.UserId,
            UserName = app.User.Name
        }).ToList();
    }
}
