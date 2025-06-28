namespace JobTrackerAPI.Models;

public class JobApplication
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int JobPostingId { get; set; } // 🔗 Link to JobPosting
    public JobPosting JobPosting { get; set; } = null!;

    public DateTime ApplicationDate { get; set; } = DateTime.UtcNow;

    public string Status { get; set; } = "Applied";
    public string Notes { get; set; } = string.Empty;

    public ICollection<StatusLog> StatusLogs { get; set; } = new List<StatusLog>();

}
