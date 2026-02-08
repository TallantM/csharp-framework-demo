using System.Collections.Generic;
using System.Linq;

namespace csharp_framework_demo.Utilities.SpecValidation;

/// <summary>
/// Represents the result of a spec-to-code validation operation
/// </summary>
public class ValidationResult
{
    /// <summary>
    /// Gets a value indicating whether the validation passed (no errors)
    /// </summary>
    public bool IsValid => !Errors.Any();

    /// <summary>
    /// Gets the list of validation errors
    /// </summary>
    public List<ValidationError> Errors { get; } = new();

    /// <summary>
    /// Gets the list of validation warnings
    /// </summary>
    public List<ValidationWarning> Warnings { get; } = new();

    /// <summary>
    /// Adds an error to the validation result
    /// </summary>
    public void AddError(string code, string message, string? location = null)
    {
        Errors.Add(new ValidationError(code, message, location));
    }

    /// <summary>
    /// Adds a warning to the validation result
    /// </summary>
    public void AddWarning(string code, string message, string? location = null)
    {
        Warnings.Add(new ValidationWarning(code, message, location));
    }

    /// <summary>
    /// Gets a formatted string representation of the validation result
    /// </summary>
    public override string ToString()
    {
        if (IsValid && !Warnings.Any())
        {
            return "Validation passed with no errors or warnings.";
        }

        var result = new System.Text.StringBuilder();

        if (Errors.Any())
        {
            result.AppendLine($"Validation failed with {Errors.Count} error(s):");
            foreach (var error in Errors)
            {
                result.AppendLine($"  - {error}");
            }
        }

        if (Warnings.Any())
        {
            result.AppendLine($"Warnings ({Warnings.Count}):");
            foreach (var warning in Warnings)
            {
                result.AppendLine($"  - {warning}");
            }
        }

        return result.ToString();
    }
}

/// <summary>
/// Represents a validation error
/// </summary>
public record ValidationError(string Code, string Message, string? Location = null)
{
    /// <summary>
    /// Gets a formatted string representation of the error
    /// </summary>
    public override string ToString()
    {
        var location = !string.IsNullOrEmpty(Location) ? $" at {Location}" : "";
        return $"[{Code}]{location}: {Message}";
    }
}

/// <summary>
/// Represents a validation warning
/// </summary>
public record ValidationWarning(string Code, string Message, string? Location = null)
{
    /// <summary>
    /// Gets a formatted string representation of the warning
    /// </summary>
    public override string ToString()
    {
        var location = !string.IsNullOrEmpty(Location) ? $" at {Location}" : "";
        return $"[{Code}]{location}: {Message}";
    }
}
