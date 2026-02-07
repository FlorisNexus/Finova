using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Malta.Validators;

/// <summary>
/// Validator for Malta Identity Card Number.
/// Format: 1 to 7 digits followed by a letter (A, B, G, H, L, M, P, Z).
/// </summary>
public partial class MaltaNationalIdValidator : NationalIdValidatorBase
{
    [GeneratedRegex(@"^\d{1,7}[ABGHLMPZ]$")]
    private static partial Regex FormatRegex();

    /// <inheritdoc/>
        public override string CountryCode => "MT";

    /// <inheritdoc/>
    protected override bool IsValidLength(string sanitized) => sanitized.Length >= 2 && sanitized.Length <= 8;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string sanitized) => FormatRegex().IsMatch(sanitized);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string sanitized)
    {
        // No known public checksum algorithm for the ID number itself.
        return ValidationResult.Success();
    }

    /// <summary>
    /// Static validation method for Maltese National ID.
    /// </summary>
        public static ValidationResult ValidateStatic(string? nationalId) => new MaltaNationalIdValidator().Validate(nationalId);
}