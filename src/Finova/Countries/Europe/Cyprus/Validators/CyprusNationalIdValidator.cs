using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Cyprus.Validators;

/// <summary>
/// Validator for Cyprus National Identity Number.
/// Format: 6 to 10 digits.
/// </summary>
public partial class CyprusNationalIdValidator : NationalIdValidatorBase
{
    /// <inheritdoc/>
        public override string CountryCode => "CY";

    /// <inheritdoc/>
    protected override bool IsValidLength(string sanitized) => sanitized.Length >= 6 && sanitized.Length <= 10;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string sanitized) => long.TryParse(sanitized, out _);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string sanitized)
    {
        // No known public checksum algorithm available for the ID number itself.
        return ValidationResult.Success();
    }

    /// <summary>
    /// Static validation method for Cypriot National ID.
    /// </summary>
        public static ValidationResult ValidateStatic(string? nationalId) => new CyprusNationalIdValidator().Validate(nationalId);
}