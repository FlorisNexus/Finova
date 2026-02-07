using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Monaco.Validators;

/// <summary>
/// Validator for Monaco National Identification Number.
/// Format: 4 to 10 digits.
/// </summary>
public partial class MonacoNationalIdValidator : NationalIdValidatorBase
{
    /// <inheritdoc/>
        public override string CountryCode => "MC";

    /// <inheritdoc/>
    protected override bool IsValidLength(string sanitized) => sanitized.Length >= 4 && sanitized.Length <= 10;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string sanitized) => long.TryParse(sanitized, out _);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string sanitized)
    {
        // No public checksum algorithm available for MC.
        return ValidationResult.Success();
    }

    /// <summary>
    /// Static validation method for Monégasque National ID.
    /// </summary>
        public static ValidationResult ValidateStatic(string? nationalId) => new MonacoNationalIdValidator().Validate(nationalId);
}