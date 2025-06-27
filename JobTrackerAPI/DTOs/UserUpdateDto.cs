using System.ComponentModel.DataAnnotations;
using JobTrackerAPI.Validators;

namespace JobTrackerAPI.DTOs;

public class UserUpdateDto
{
    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Role is required.")]
    [AllowedRoles("User", "Admin")]
    public string Role { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; } = string.Empty;
}
