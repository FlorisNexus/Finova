using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.SanMarino.Validators;

/// <summary>
/// Validator for San Marino National Identification Number (SSI).
/// Format: 5 digits.
/// </summary>
public partial class SanMarinoNationalIdValidator : NationalIdValidatorBase
{
    /// <inheritdoc/>
        public override string CountryCode => "SM";

    /// <inheritdoc/>
    protected override bool IsValidLength(string sanitized) => sanitized.Length == 5;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string sanitized) => long.TryParse(sanitized, out _);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string sanitized)
    {
        // No public checksum algorithm available for SM.
        return ValidationResult.Success();
    }

    /// <summary>
    /// Static validation method for San Marino SSI.
    /// </summary>
        public static ValidationResult ValidateStatic(string? nationalId) => new SanMarinoNationalIdValidator().Validate(nationalId);
}