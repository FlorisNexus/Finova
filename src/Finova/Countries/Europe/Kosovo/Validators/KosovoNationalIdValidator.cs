using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Kosovo.Validators;

/// <summary>
/// Validator for Kosovo National Identity Number (Letërnjoftim).
/// Format: 10 digits.
/// </summary>
public partial class KosovoNationalIdValidator : NationalIdValidatorBase
{
    /// <inheritdoc/>
        public override string CountryCode => "XK";

    /// <inheritdoc/>
    protected override bool IsValidLength(string sanitized) => sanitized.Length == 10;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string sanitized) => long.TryParse(sanitized, out _);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string sanitized)
    {
        // No public checksum algorithm available for XK currently.
        return ValidationResult.Success();
    }

    /// <summary>
    /// Static validation method for Kosovo National ID.
    /// </summary>
        public static ValidationResult ValidateStatic(string? nationalId) => new KosovoNationalIdValidator().Validate(nationalId);
}