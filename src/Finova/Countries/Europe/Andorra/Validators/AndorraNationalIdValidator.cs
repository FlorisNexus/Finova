using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Andorra.Validators;

/// <summary>
/// Validator for Andorran National Identity Number (NIA - Número d'Identificació Administrativa).
/// Format: 8 characters. [A-Z] + 6 digits + [A-Z].
/// </summary>
public partial class AndorraNationalIdValidator : NationalIdValidatorBase
{
    [GeneratedRegex(@"^[A-Z]\d{6}[A-Z]$")]
    private static partial Regex FormatRegex();

    /// <inheritdoc/>
        public override string CountryCode => "AD";

    /// <inheritdoc/>
    protected override bool IsValidLength(string sanitized) => sanitized.Length == 8;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string sanitized) => FormatRegex().IsMatch(sanitized);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string sanitized)
    {
        // No public checksum algorithm available.
        return ValidationResult.Success();
    }

    /// <summary>
    /// Static validation method for Andorran National ID.
    /// </summary>
        public static ValidationResult ValidateStatic(string? nationalId) => new AndorraNationalIdValidator().Validate(nationalId);
}