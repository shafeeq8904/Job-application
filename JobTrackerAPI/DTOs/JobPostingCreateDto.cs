using System.ComponentModel.DataAnnotations;

namespace JobTrackerAPI.DTOs;

public class JobPostingCreateDto
{
    [Required]
    public string JobTitle { get; set; } = string.Empty;

    [Required]
    public string CompanyName { get; set; } = string.Empty;

    [Required]
    public string Location { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;
}
