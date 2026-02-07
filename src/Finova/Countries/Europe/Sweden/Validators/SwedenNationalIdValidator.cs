using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Sweden.Validators;

/// <summary>
/// Validator for Sweden Personal Identity Number (Personnummer).
/// Format: 10 or 12 digits.
/// </summary>
public partial class SwedenNationalIdValidator : NationalIdValidatorBase
{
    /// <inheritdoc/>
        public override string CountryCode => "SE";

    /// <inheritdoc/>
    protected override bool IsValidLength(string sanitized) => sanitized.Length == 10 || sanitized.Length == 12;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string sanitized) => long.TryParse(sanitized, out _);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string sanitized)
    {
        string toCheck = sanitized.Length == 12 ? sanitized.Substring(2) : sanitized;

        return ChecksumHelper.ValidateLuhn(toCheck)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }

    /// <summary>
    /// Static validation method for Swedish Personnummer.
    /// </summary>
        public static ValidationResult ValidateStatic(string? nationalId) => new SwedenNationalIdValidator().Validate(nationalId);
}