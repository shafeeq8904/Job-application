namespace JobTrackerAPI.DTOs;

public class JobPostingResponseDto
{
    public int Id { get; set; }
    public string JobTitle { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime PostedDate { get; set; }
}
