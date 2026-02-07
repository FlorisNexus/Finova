using System.Globalization;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Luxembourg.Validators;

/// <summary>
/// Validator for Luxembourg National Identification Number (Matricule).
/// Format: 13 digits (YYYYMMDD + 5 more digits).
/// </summary>
public partial class LuxembourgNationalIdValidator : NationalIdValidatorBase
{
    /// <inheritdoc/>
        public override string CountryCode => "LU";

    /// <inheritdoc/>
    protected override bool IsValidLength(string sanitized) => sanitized.Length == 13;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string sanitized)
    {
        if (!long.TryParse(sanitized, out _))
        {
            return false;
        }

        // First 8 digits are YYYYMMDD
        string dateStr = sanitized.Substring(0, 8);
        return DateTime.TryParseExact(dateStr, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
    }

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string sanitized)
    {
        // Checksum: First 11 digits % 97 == Last 2 digits
        long number = long.Parse(sanitized.Substring(0, 11));
        int expectedCheckDigits = (int)(number % 97);
        int actualCheckDigits = int.Parse(sanitized.Substring(11, 2));

        return expectedCheckDigits == actualCheckDigits
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }

    /// <summary>
    /// Static validation method for Luxembourgish Matricule.
    /// </summary>
        public static ValidationResult ValidateStatic(string? nationalId) => new LuxembourgNationalIdValidator().Validate(nationalId);
}