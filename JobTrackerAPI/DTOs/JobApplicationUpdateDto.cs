using System.ComponentModel.DataAnnotations;
using JobTrackerAPI.Validators;

namespace JobTrackerAPI.DTOs;

public class JobApplicationUpdateDto
{
    [Required(ErrorMessage = "Status is required.")]
    [AllowedStatus]
    public string Status { get; set; } = string.Empty;

    public string Notes { get; set; } = string.Empty;
}