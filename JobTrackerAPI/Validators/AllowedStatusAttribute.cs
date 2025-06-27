using System.ComponentModel.DataAnnotations;

namespace JobTrackerAPI.Validators;

public class AllowedStatusAttribute : ValidationAttribute
{
    private readonly string[] _allowedStatuses = new[] { "Applied", "Interview Scheduled", "Offered", "Rejected" };

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is string status && _allowedStatuses.Contains(status))
            return ValidationResult.Success;

        return new ValidationResult($"Status must be one of: {string.Join(", ", _allowedStatuses)}");
    }
}
