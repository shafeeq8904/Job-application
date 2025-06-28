using System.ComponentModel.DataAnnotations;

namespace JobTrackerAPI.DTOs;

public class JobApplicationCreateDto
{
    [Required]
    public int UserId { get; set; }

    [Required]
    public int JobPostingId { get; set; }

    public string Notes { get; set; } = string.Empty;
}
