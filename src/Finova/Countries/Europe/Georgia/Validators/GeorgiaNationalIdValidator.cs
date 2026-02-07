using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Georgia.Validators;

/// <summary>
/// Validator for Georgia Personal Number (Piradi nomeri).
/// Format: 11 digits.
/// </summary>
public partial class GeorgiaNationalIdValidator : NationalIdValidatorBase
{
    /// <inheritdoc/>
        public override string CountryCode => "GE";

    /// <inheritdoc/>
    protected override bool IsValidLength(string sanitized) => sanitized.Length == 11;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string sanitized) => long.TryParse(sanitized, out _);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string sanitized)
    {
        // No public checksum algorithm available for GE.
        return ValidationResult.Success();
    }

    /// <summary>
    /// Static validation method for Georgian National ID.
    /// </summary>
        public static ValidationResult ValidateStatic(string? nationalId) => new GeorgiaNationalIdValidator().Validate(nationalId);
}