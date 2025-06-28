namespace JobTrackerAPI.Models;

public class JobPosting
{
    public int Id { get; set; }

    public string JobTitle { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public DateTime PostedDate { get; set; } = DateTime.UtcNow;

    public ICollection<JobApplication> Applications { get; set; } = new List<JobApplication>();
}
