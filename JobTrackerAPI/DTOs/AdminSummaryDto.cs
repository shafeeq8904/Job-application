namespace JobTrackerAPI.DTOs;

public class AdminSummaryDto
{
    public int TotalApplications { get; set; }
    public Dictionary<string, int> StatusCounts { get; set; } = new();
    public List<UserApplicationCountDto> ApplicationsPerUser { get; set; } = new();
}

public class UserApplicationCountDto
{
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public int ApplicationCount { get; set; }
}
