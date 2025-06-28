using JobTrackerAPI.DTOs;
using JobTrackerAPI.Models;
using JobTrackerAPI.Repositories;
using Microsoft.EntityFrameworkCore;

public class JobApplicationService : IJobApplicationService
{
    private readonly IRepository<JobApplication> _applicationRepo;
    private readonly IRepository<User> _userRepo;

    public JobApplicationService(IRepository<JobApplication> applicationRepo, IRepository<User> userRepo)
    {
        _applicationRepo = applicationRepo;
        _userRepo = userRepo;
    }

    public async Task<JobApplicationResponseDto> CreateApplicationAsync(JobApplicationCreateDto dto)
    {
        var user = await _userRepo.GetByIdAsync(dto.UserId);

        if (user == null)
        {
            throw new Exception("Invalid user. User does not exist.");
        }

        if (user.Role != "User" && user.Role != "Admin")
        {
            throw new Exception("Unauthorized role.");
        }

        var app = new JobApplication
        {
            UserId = dto.UserId,
            JobTitle = dto.JobTitle,
            CompanyName = dto.CompanyName,
            Location = dto.Location,
            ApplicationDate = dto.ApplicationDate.ToUniversalTime() ,
            Status = dto.Status,
        };

        await _applicationRepo.AddAsync(app);
        await _applicationRepo.SaveAsync();

        // Manual mapping to response DTO
        return new JobApplicationResponseDto
        {
            Id = app.Id,
            UserId = user.Id,
            UserName = user.Name,
            JobTitle = app.JobTitle,
            CompanyName = app.CompanyName,
            Location = app.Location,
            ApplicationDate = app.ApplicationDate,
            Status = app.Status,
        };
    }

    // Fetches all job applications for a user or all applications if the user is an admin
   public async Task<List<JobApplicationResponseDto>> GetApplicationsAsync(
    int userId, string role, string? status = null, string? companyName = null)
    {
        IEnumerable<JobApplication> apps;

        if (role == "Admin")
        {
            apps = await _applicationRepo.GetAllAsync(q => q.Include(a => a.User));
        }
        else
        {
            apps = await _applicationRepo.FindAsync(
                a => a.UserId == userId,
                q => q.Include(a => a.User)
            );
        }

        // ✅ Apply optional filters
        if (!string.IsNullOrWhiteSpace(status))
        {
            apps = apps.Where(a => a.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(companyName))
        {
            apps = apps.Where(a => a.CompanyName.Contains(companyName, StringComparison.OrdinalIgnoreCase));
        }

        return apps.Select(app => new JobApplicationResponseDto
        {
            Id = app.Id,
            UserId = app.UserId,
            UserName = app.User.Name,
            JobTitle = app.JobTitle,
            CompanyName = app.CompanyName,
            Location = app.Location,
            ApplicationDate = app.ApplicationDate,
            Status = app.Status,
            Notes = app.Notes
        }).ToList();
    }


    // Fetches a single job application by ID
    public async Task<JobApplicationResponseDto?> GetByIdAsync(int id)
    {
        var apps = await _applicationRepo.FindAsync(
            a => a.Id == id,
            q => q.Include(a => a.User)
        );

        var app = apps.FirstOrDefault();
        if (app == null) return null;

        return new JobApplicationResponseDto
        {
            Id = app.Id,
            UserId = app.UserId,
            UserName = app.User.Name,
            JobTitle = app.JobTitle,
            CompanyName = app.CompanyName,
            Location = app.Location,
            ApplicationDate = app.ApplicationDate,
            Status = app.Status,
            Notes = app.Notes
        };
    }

    public async Task<JobApplicationResponseDto?> UpdateAsync(int id, JobApplicationUpdateDto dto)
    {
        var apps = await _applicationRepo.FindAsync(
            a => a.Id == id,
            q => q.Include(a => a.User).Include(a => a.StatusLogs)
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
            JobTitle = app.JobTitle,
            CompanyName = app.CompanyName,
            Location = app.Location,
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
