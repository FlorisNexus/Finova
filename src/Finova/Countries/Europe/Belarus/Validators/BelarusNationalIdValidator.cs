using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Belarus.Validators;

/// <summary>
/// Validator for Belarus Personal Identification Number (Ientifikatsionny nomer).
/// Format: 14 characters.
/// </summary>
public partial class BelarusNationalIdValidator : NationalIdValidatorBase
{
    [GeneratedRegex(@"^[1-6]\d{6}[ABCKEMH]\d{3}[A-Z]{2}\d$")]
    private static partial Regex FormatRegex();

    /// <inheritdoc/>
        public override string CountryCode => "BY";

    /// <inheritdoc/>
    protected override bool IsValidLength(string sanitized) => sanitized.Length == 14;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string sanitized)
    {
        if (!FormatRegex().IsMatch(sanitized))
        {
            return false;
        }

        // Date Validation
        int centuryCode = sanitized[0] - '0';
        int day = int.Parse(sanitized.Substring(1, 2));
        int month = int.Parse(sanitized.Substring(3, 2));
        int yearPart = int.Parse(sanitized.Substring(5, 2));

        int century = centuryCode switch
        {
            1 or 2 => 1800,
            3 or 4 => 1900,
            5 or 6 => 2000,
            _ => 0
        };

        return DateHelper.IsValidDate(century + yearPart, month, day);
    }

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string sanitized)
    {
        // Checksum calculation could be added here.
        return ValidationResult.Success();
    }

    /// <summary>
    /// Static validation method for Belarus National ID.
    /// </summary>
        public static ValidationResult ValidateStatic(string? nationalId) => new BelarusNationalIdValidator().Validate(nationalId);
}