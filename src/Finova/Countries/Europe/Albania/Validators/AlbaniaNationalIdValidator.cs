using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Albania.Validators;

/// <summary>
/// Validator for Albanian National Identity Number (NID / Letërnjoftim).
/// Format: 10 characters. [A-Z] + 8 digits + [A-Z].
/// </summary>
public partial class AlbaniaNationalIdValidator : NationalIdValidatorBase
{
    [GeneratedRegex(@"^[A-Z]\d{8}[A-Z]$")]
    private static partial Regex FormatRegex();

    /// <inheritdoc/>
        public override string CountryCode => "AL";

    /// <inheritdoc/>
    protected override bool IsValidLength(string sanitized) => sanitized.Length == 10;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string sanitized)
    {
        if (!FormatRegex().IsMatch(sanitized))
        {
            return false;
        }

        // Date Validation
        char decadeChar = sanitized[0];
        int yearPart = int.Parse(sanitized.Substring(1, 2));
        int monthPart = int.Parse(sanitized.Substring(3, 2));
        int dayPart = int.Parse(sanitized.Substring(5, 2));

        int decadeBase = GetDecadeBase(decadeChar);
        if (decadeBase == -1)
        {
            return false;
        }

        int century = (decadeBase >= 2000) ? 2000 : 1900;
        int fullYear = century + yearPart;

        int month = monthPart > 50 ? monthPart - 50 : monthPart;

        return DateHelper.IsValidDate(fullYear, month, dayPart);
    }

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string sanitized)
    {
        // No public checksum algorithm available.
        return ValidationResult.Success();
    }

    private static int GetDecadeBase(char c)
    {
        return c switch
        {
            'A' => 1900,
            'B' => 1910,
            'C' => 1920,
            'D' => 1930,
            'E' => 1940,
            'F' => 1950,
            'G' => 1960,
            'H' => 1970,
            'I' => 1980,
            'J' => 1990,
            'K' => 2000,
            'L' => 2010,
            'M' => 2020,
            'N' => 2030,
            'P' => 2040,
            _ => -1
        };
    }

    /// <summary>
    /// Static validation method for Albanian National ID.
    /// </summary>
        public static ValidationResult ValidateStatic(string? nationalId) => new AlbaniaNationalIdValidator().Validate(nationalId);
}