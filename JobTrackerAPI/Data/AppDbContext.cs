using Microsoft.EntityFrameworkCore;
using JobTrackerAPI.Models;

namespace JobTrackerAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<User> Users { get; set; }
    public DbSet<JobApplication> JobApplications { get; set; }
    public DbSet<StatusLog> StatusLogs { get; set; }

    public DbSet<JobPosting> JobPostings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany(u => u.Applications)
            .WithOne(a => a.User)
            .HasForeignKey(a => a.UserId);

        modelBuilder.Entity<JobApplication>()
            .HasMany(a => a.StatusLogs)
            .WithOne(s => s.Application)
            .HasForeignKey(s => s.ApplicationId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<JobApplication>()
            .HasOne(a => a.JobPosting)
            .WithMany(p => p.Applications)
            .HasForeignKey(a => a.JobPostingId);
            
        modelBuilder.Entity<JobApplication>()
            .HasIndex(a => new { a.UserId, a.JobPostingId })
            .IsUnique();
    }
}
