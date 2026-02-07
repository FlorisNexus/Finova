using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Iceland.Validators;

/// <summary>
/// Validator for Iceland Kennitala (National ID).
/// Format: 10 digits.
/// </summary>
public partial class IcelandKennitalaValidator : NationalIdValidatorBase
{
    [GeneratedRegex(@"^\d{10}$")]
    private static partial Regex FormatRegex();

    /// <inheritdoc/>
        public override string CountryCode => "IS";

    /// <inheritdoc/>
    protected override bool IsValidLength(string sanitized) => sanitized.Length == 10;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string sanitized)
    {
        if (!FormatRegex().IsMatch(sanitized))
        {
            return false;
        }

        // Date Validation
        int day = int.Parse(sanitized.Substring(0, 2));
        int month = int.Parse(sanitized.Substring(2, 2));

        // Handle organization numbers (day + 40)
        if (day > 40)
        {
            day -= 40;
        }

        return day >= 1 && day <= 31 && month >= 1 && month <= 12;
    }

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string sanitized)
    {
        // Weights: 3, 2, 7, 6, 5, 4, 3, 2
        int[] weights = { 3, 2, 7, 6, 5, 4, 3, 2 };
        int sum = 0;
        for (int i = 0; i < 8; i++)
        {
            sum += (sanitized[i] - '0') * weights[i];
        }

        int remainder = sum % 11;
        int checkDigit = 11 - remainder;
        if (checkDigit == 11)
        {
            checkDigit = 0;
        }

        if (checkDigit == 10)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
        }

        return checkDigit == (sanitized[8] - '0')
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }

    /// <summary>
    /// Static validation method for Iceland Kennitala.
    /// </summary>
    public static ValidationResult ValidateStatic(string? number) => new IcelandKennitalaValidator().Validate(number);

    /// <summary>
    /// Formats an Iceland Kennitala.
    /// </summary>
    public static string? Format(string? instance)
    {
        var validator = new IcelandKennitalaValidator();
        var normalized = validator.Parse(instance);
        if (normalized == null || normalized.Length != 10)
        {
            return normalized;
        }
        return $"{normalized.Substring(0, 6)}-{normalized.Substring(6)}";
    }
}