namespace JobTrackerAPI.Models;

public class StatusLog
{
    public int Id { get; set; }

    public int ApplicationId { get; set; }
    public JobApplication Application { get; set; } = null!;

    public string NewStatus { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}
