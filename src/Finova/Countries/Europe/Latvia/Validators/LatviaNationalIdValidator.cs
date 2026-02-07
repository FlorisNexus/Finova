using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Latvia.Validators;

/// <summary>
/// Validator for Latvia Personal Code (Personas kods).
/// Format: 11 digits.
/// </summary>
public partial class LatviaNationalIdValidator : NationalIdValidatorBase
{
    private static readonly int[] Weights = { 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };

    /// <inheritdoc/>
        public override string CountryCode => "LV";

    /// <inheritdoc/>
    protected override bool IsValidLength(string sanitized) => sanitized.Length == 11;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string sanitized)
    {
        if (!long.TryParse(sanitized, out _))
        {
            return false;
        }

        // New format: starts with 32. No date info.
        if (sanitized.StartsWith("32"))
        {
            return true;
        }

        // Old format: DDMMYYXXXXX
        int day = int.Parse(sanitized.Substring(0, 2));
        int month = int.Parse(sanitized.Substring(2, 2));
        int yearPart = int.Parse(sanitized.Substring(4, 2));
        int centuryDigit = sanitized[6] - '0';

        int century = centuryDigit switch
        {
            0 => 1800,
            1 => 1900,
            2 => 2000,
            _ => 0
        };

        if (century != 0)
        {
            return DateHelper.IsValidDate(century + yearPart, month, day);
        }

        return month >= 1 && month <= 12 && day >= 1 && day <= 31;
    }

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string sanitized)
    {
        // New format (starting with 32) has no checksum.
        if (sanitized.StartsWith("32"))
        {
            return ValidationResult.Success();
        }

        int sum = 0;
        for (int i = 0; i < 10; i++)
        {
            sum += (sanitized[i] - '0') * Weights[i];
        }

        int remainder = sum % 11;
        int checkDigit = (1 - remainder + 11) % 11;

        if (checkDigit == 10)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
        }

        return checkDigit == (sanitized[10] - '0')
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }

    /// <summary>
    /// Static validation method for Latvian Personal Code.
    /// </summary>
        public static ValidationResult ValidateStatic(string? nationalId) => new LatviaNationalIdValidator().Validate(nationalId);
}