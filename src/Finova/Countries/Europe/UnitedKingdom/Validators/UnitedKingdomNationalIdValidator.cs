using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.UnitedKingdom.Validators;

/// <summary>
/// Validator for United Kingdom National Insurance Number (NINO).
/// Format: 9 characters (2 letters, 6 digits, 1 letter).
/// </summary>
public partial class UnitedKingdomNationalIdValidator : NationalIdValidatorBase
{
    [GeneratedRegex(@"^[A-Z]{2}[0-9]{6}[A-Z]$")]
    private static partial Regex FormatRegex();

    /// <inheritdoc/>
        public override string CountryCode => "GB";

    /// <inheritdoc/>
    protected override bool IsValidLength(string sanitized) => sanitized.Length == 9;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string sanitized)
    {
        if (!FormatRegex().IsMatch(sanitized))
        {
            return false;
        }

        // Specific invalid prefixes
        string prefix = sanitized.Substring(0, 2);
        string[] invalidPrefixes = { "BG", "GB", "NK", "KN", "TN", "NT", "ZZ" };
        foreach (var invalid in invalidPrefixes)
        {
            if (prefix == invalid)
            {
                return false;
            }
        }

        // Second letter cannot be O
        return sanitized[1] != 'O';
    }

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string sanitized)
    {
        // No public checksum algorithm available for NINO.
        return ValidationResult.Success();
    }

    /// <summary>
    /// Static validation method for UK NINO.
    /// </summary>
        public static ValidationResult ValidateStatic(string? nationalId) => new UnitedKingdomNationalIdValidator().Validate(nationalId);
}