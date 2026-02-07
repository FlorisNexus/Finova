using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Azerbaijan.Validators;

/// <summary>
/// Validator for Azerbaijan Personal Identification Number (PIN / FİN).
/// Format: 7 alphanumeric characters.
/// </summary>
public partial class AzerbaijanNationalIdValidator : NationalIdValidatorBase
{
    [GeneratedRegex(@"^[A-Z0-9]{7}$")]
    private static partial Regex FormatRegex();

    /// <inheritdoc/>
        public override string CountryCode => "AZ";

    /// <inheritdoc/>
    protected override bool IsValidLength(string sanitized) => sanitized.Length == 7;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string sanitized) => FormatRegex().IsMatch(sanitized);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string sanitized)
    {
        // No public checksum algorithm available.
        return ValidationResult.Success();
    }

    /// <summary>
    /// Static validation method for Azerbaijan National ID.
    /// </summary>
        public static ValidationResult ValidateStatic(string? nationalId) => new AzerbaijanNationalIdValidator().Validate(nationalId);
}