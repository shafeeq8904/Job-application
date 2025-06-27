namespace JobTrackerAPI.DTOs;

public class StatusLogResponseDto
{
    public string NewStatus { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}
