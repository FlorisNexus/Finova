using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Gibraltar.Validators;

/// <summary>
/// Validator for Gibraltar Identity Card Number.
/// Format: 5 to 12 characters.
/// </summary>
public partial class GibraltarNationalIdValidator : NationalIdValidatorBase
{
    /// <inheritdoc/>
        public override string CountryCode => "GI";

    /// <inheritdoc/>
    protected override bool IsValidLength(string sanitized) => sanitized.Length >= 5 && sanitized.Length <= 12;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string sanitized) => true; // Basic length check is enough for GI currently.

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string sanitized)
    {
        // No public checksum algorithm available for GI.
        return ValidationResult.Success();
    }

    /// <summary>
    /// Static validation method for Gibraltar National ID.
    /// </summary>
        public static ValidationResult ValidateStatic(string? nationalId) => new GibraltarNationalIdValidator().Validate(nationalId);
}