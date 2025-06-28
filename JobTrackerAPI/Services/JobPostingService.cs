using JobTrackerAPI.Data;
using JobTrackerAPI.DTOs;
using JobTrackerAPI.Models;
using Microsoft.EntityFrameworkCore;

public class JobPostingService : IJobPostingService
{
    private readonly AppDbContext _context;

    public JobPostingService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<JobPostingResponseDto>> GetAllAsync()
    {
        var jobs = await _context.JobPostings
            .OrderByDescending(j => j.PostedDate)
            .ToListAsync();

        return jobs.Select(j => new JobPostingResponseDto
        {
            Id = j.Id,
            JobTitle = j.JobTitle,
            CompanyName = j.CompanyName,
            Location = j.Location,
            Description = j.Description,
            PostedDate = j.PostedDate
        }).ToList();
    }

    public async Task<JobPostingResponseDto?> GetByIdAsync(int id)
    {
        var job = await _context.JobPostings.FindAsync(id);
        if (job == null) return null;

        return new JobPostingResponseDto
        {
            Id = job.Id,
            JobTitle = job.JobTitle,
            CompanyName = job.CompanyName,
            Location = job.Location,
            Description = job.Description,
            PostedDate = job.PostedDate
        };
    }

    public async Task<JobPostingResponseDto> CreateAsync(JobPostingCreateDto dto)
    {
        var job = new JobPosting
        {
            JobTitle = dto.JobTitle,
            CompanyName = dto.CompanyName,
            Location = dto.Location,
            Description = dto.Description,
            PostedDate = DateTime.UtcNow
        };

        _context.JobPostings.Add(job);
        await _context.SaveChangesAsync();

        return new JobPostingResponseDto
        {
            Id = job.Id,
            JobTitle = job.JobTitle,
            CompanyName = job.CompanyName,
            Location = job.Location,
            Description = job.Description,
            PostedDate = job.PostedDate
        };
    }

    public async Task<JobPostingResponseDto?> UpdateAsync(int id, JobPostingCreateDto dto)
    {
        var job = await _context.JobPostings.FindAsync(id);
        if (job == null) return null;

        job.JobTitle = dto.JobTitle;
        job.CompanyName = dto.CompanyName;
        job.Location = dto.Location;
        job.Description = dto.Description;

        await _context.SaveChangesAsync();

        return new JobPostingResponseDto
        {
            Id = job.Id,
            JobTitle = job.JobTitle,
            CompanyName = job.CompanyName,
            Location = job.Location,
            Description = job.Description,
            PostedDate = job.PostedDate
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var job = await _context.JobPostings.FindAsync(id);
        if (job == null) return false;

        _context.JobPostings.Remove(job);
        await _context.SaveChangesAsync();
        return true;
    }

}
