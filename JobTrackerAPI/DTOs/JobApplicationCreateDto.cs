using System.ComponentModel.DataAnnotations;
using JobTrackerAPI.Validators;

namespace JobTrackerAPI.DTOs;

public class JobApplicationCreateDto
{
    [Required]
    public int UserId { get; set; }

    [Required(ErrorMessage = "Job title is required.")]
    public string JobTitle { get; set; } = string.Empty;

    [Required(ErrorMessage = "Company name is required.")]
    public string CompanyName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Location is required.")]
    public string Location { get; set; } = string.Empty;

    [Required(ErrorMessage = "Application date is required.")]
    public DateTime ApplicationDate { get; set; }

    [Required(ErrorMessage = "Status is required.")]
    [AllowedStatus]
    public string Status { get; set; } = string.Empty;

    public string Notes { get; set; } = string.Empty;
}
