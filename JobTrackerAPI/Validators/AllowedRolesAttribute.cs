using System.ComponentModel.DataAnnotations;

namespace JobTrackerAPI.Validators;

public class AllowedRolesAttribute : ValidationAttribute
{
    private readonly string[] _allowedRoles;

    public AllowedRolesAttribute(params string[] allowedRoles)
    {
        _allowedRoles = allowedRoles;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is string role && _allowedRoles.Contains(role))
            return ValidationResult.Success;

        return new ValidationResult($"Role must be one of: {string.Join(", ", _allowedRoles)}");
    }
}
