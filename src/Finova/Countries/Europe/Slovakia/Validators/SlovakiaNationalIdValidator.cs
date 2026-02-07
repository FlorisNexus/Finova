using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Slovakia.Validators;

/// <summary>
/// Validator for Slovak Birth Number (Rodné číslo).
/// Format: 9 or 10 digits.
/// </summary>
public partial class SlovakiaNationalIdValidator : NationalIdValidatorBase
{
    /// <inheritdoc/>
        public override string CountryCode => "SK";

    /// <inheritdoc/>
    protected override bool IsValidLength(string sanitized) => sanitized.Length == 9 || sanitized.Length == 10;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string sanitized) => long.TryParse(sanitized, out _);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string sanitized)
    {
        if (sanitized.Length == 10)
        {
            long number = long.Parse(sanitized);
            return number % 11 == 0
                ? ValidationResult.Success()
                : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
        }

        // Before 1954: 9 digits, no checksum.
        return ValidationResult.Success();
    }

    /// <summary>
    /// Static validation method for Slovak National ID.
    /// </summary>
        public static ValidationResult ValidateStatic(string? nationalId) => new SlovakiaNationalIdValidator().Validate(nationalId);
}