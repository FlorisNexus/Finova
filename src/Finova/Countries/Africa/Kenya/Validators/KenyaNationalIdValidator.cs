using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Africa.Kenya.Validators;

/// <summary>
/// Validator for Kenyan National ID numbers.
/// Format: 7 to 9 digits.
/// </summary>
public partial class KenyaNationalIdValidator : NationalIdValidatorBase
{
    /// <inheritdoc/>
        public override string CountryCode => "KE";

    /// <inheritdoc/>
    protected override bool IsValidLength(string sanitized) => sanitized.Length >= 7 && sanitized.Length <= 9;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string sanitized) => long.TryParse(sanitized, out _);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string sanitized)
    {
        // No public checksum algorithm available for KE currently.
        return ValidationResult.Success();
    }

    /// <summary>
    /// Static validation method for Kenyan National ID.
    /// </summary>
    public static ValidationResult ValidateStatic(string? idNumber) => new KenyaNationalIdValidator().Validate(idNumber);
}