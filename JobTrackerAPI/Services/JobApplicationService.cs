using JobTrackerAPI.DTOs;
using JobTrackerAPI.Models;
using JobTrackerAPI.Repositories;
using Microsoft.EntityFrameworkCore;

public class JobApplicationService : IJobApplicationService
{
    private readonly IRepository<JobApplication> _applicationRepo;
    private readonly IRepository<User> _userRepo;
    private readonly IRepository<JobPosting> _jobPostingRepo;

    public JobApplicationService(IRepository<JobApplication> applicationRepo, IRepository<User> userRepo, IRepository<JobPosting> jobPostingRepo)
    {
        _applicationRepo = applicationRepo;
        _userRepo = userRepo;
        _jobPostingRepo = jobPostingRepo;
    }

    public async Task<JobApplicationResponseDto> CreateApplicationAsync(JobApplicationCreateDto dto)
    {
        var user = await _userRepo.GetByIdAsync(dto.UserId);
        if (user == null)
            throw new Exception("Invalid user.");

        // 🚫 Prevent duplicate applications
        var existing = await _applicationRepo.FindAsync(
            a => a.UserId == dto.UserId && a.JobPostingId == dto.JobPostingId
        );
        if (existing.Any())
            throw new Exception("You have already applied to this job.");

        var jobPosting = await _jobPostingRepo.GetByIdAsync(dto.JobPostingId);
        if (jobPosting == null)
            throw new Exception("Invalid job posting.");

        var app = new JobApplication
        {
            UserId = dto.UserId,
            JobPostingId = dto.JobPostingId,
            ApplicationDate = DateTime.UtcNow,
            Status = "Applied",
            Notes = dto.Notes
        };

        await _applicationRepo.AddAsync(app);
        await _applicationRepo.SaveAsync();

        // Load job posting and user info
        var added = (await _applicationRepo.FindAsync(
            a => a.Id == app.Id,
            q => q.Include(x => x.User).Include(x => x.JobPosting)
        )).First();

        return new JobApplicationResponseDto
        {
            Id = added.Id,
            UserId = added.UserId,
            UserName = added.User.Name,
            JobPostingId = added.JobPostingId,
            JobTitle = added.JobPosting.JobTitle,
            CompanyName = added.JobPosting.CompanyName,
            Location = added.JobPosting.Location,
            ApplicationDate = added.ApplicationDate,
            Status = added.Status,
            Notes = added.Notes
        };
    }

    // Fetches all job applications for a user or all applications if the user is an admin
   public async Task<List<JobApplicationResponseDto>> GetApplicationsAsync(
    int userId, string role, string? status = null, string? companyName = null)
    {
        IEnumerable<JobApplication> apps;

        if (role == "Admin")
        {
            apps = await _applicationRepo.GetAllAsync(q =>
                q.Include(a => a.User).Include(a => a.JobPosting));
        }
        else
        {
            apps = await _applicationRepo.FindAsync(
                a => a.UserId == userId,
                q => q.Include(a => a.User).Include(a => a.JobPosting)
            );
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            apps = apps.Where(a => a.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(companyName))
        {
            apps = apps.Where(a => a.JobPosting.CompanyName.Contains(companyName, StringComparison.OrdinalIgnoreCase));
        }

        return apps.Select(app => new JobApplicationResponseDto
        {
            Id = app.Id,
            UserId = app.UserId,
            UserName = app.User.Name,
            JobPostingId = app.JobPostingId,
            JobTitle = app.JobPosting.JobTitle,
            CompanyName = app.JobPosting.CompanyName,
            Location = app.JobPosting.Location,
            ApplicationDate = app.ApplicationDate,
            Status = app.Status,
            Notes = app.Notes
        }).ToList();
    }


    // Fetches a single job application by ID
    public async Task<JobApplicationResponseDto?> GetByIdAsync(int id, int userId, string role)
    {
        var apps = await _applicationRepo.FindAsync(
            a => a.Id == id,
            q => q.Include(a => a.User).Include(a => a.JobPosting)
        );

        var app = apps.FirstOrDefault();
        if (app == null)
            return null;

        // 🔒 Access Control: user can only access their own applications
        if (role != "Admin" && app.UserId != userId)
            return null;

        return new JobApplicationResponseDto
        {
            Id = app.Id,
            UserId = app.UserId,
            UserName = app.User.Name,
            JobPostingId = app.JobPostingId,
            JobTitle = app.JobPosting.JobTitle,
            CompanyName = app.JobPosting.CompanyName,
            Location = app.JobPosting.Location,
            ApplicationDate = app.ApplicationDate,
            Status = app.Status,
            Notes = app.Notes
        };
    }

    public async Task<JobApplicationResponseDto?> UpdateAsync(int id, JobApplicationUpdateDto dto)
    {
        var apps = await _applicationRepo.FindAsync(
        a => a.Id == id,
        q => q.Include(a => a.User)
              .Include(a => a.StatusLogs)
              .Include(a => a.JobPosting)
    );

        var app = apps.FirstOrDefault();
        if (app == null) return null;

        if (app.Status != dto.Status)
        {
            app.StatusLogs.Add(new StatusLog
            {
                ApplicationId = app.Id,
                NewStatus = dto.Status,
                Timestamp = DateTime.UtcNow
            });
        }

        app.Status = dto.Status;
        app.Notes = dto.Notes;

        _applicationRepo.Update(app);
        await _applicationRepo.SaveAsync();

        return new JobApplicationResponseDto
        {
            Id = app.Id,
            UserId = app.UserId,
            UserName = app.User.Name,
            JobPostingId = app.JobPostingId,
            JobTitle = app.JobPosting.JobTitle,
            CompanyName = app.JobPosting.CompanyName,
            Location = app.JobPosting.Location,
            ApplicationDate = app.ApplicationDate,
            Status = app.Status,
            Notes = app.Notes
        };
    }

    public async Task<bool> DeleteAsync(int id, int userId, string role)
    {
        IEnumerable<JobApplication> apps;

        if (role == "Admin")
        {
            apps = await _applicationRepo.FindAsync(
                a => a.Id == id,
                q => q.Include(a => a.StatusLogs)
            );
        }
        else
        {
            apps = await _applicationRepo.FindAsync(
                a => a.Id == id && a.UserId == userId,
                q => q.Include(a => a.StatusLogs)
            );
        }

        var app = apps.FirstOrDefault();
        if (app == null) return false;

        _applicationRepo.Remove(app);
        await _applicationRepo.SaveAsync();

        return true;
    }


}
