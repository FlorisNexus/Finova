using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Greece.Validators;

/// <summary>
/// Validator for Greek Social Security Number (AMKA).
/// Format: 11 digits.
/// </summary>
public partial class GreeceNationalIdValidator : NationalIdValidatorBase
{
    /// <inheritdoc/>
        public override string CountryCode => "GR";

    /// <inheritdoc/>
    protected override bool IsValidLength(string sanitized) => sanitized.Length == 11;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string sanitized) => long.TryParse(sanitized, out _);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string sanitized)
    {
        return ChecksumHelper.ValidateLuhn(sanitized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }

    /// <summary>
    /// Static validation method for Greek National ID.
    /// </summary>
        public static ValidationResult ValidateStatic(string? input) => new GreeceNationalIdValidator().Validate(input);
}