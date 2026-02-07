using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Bulgaria.Validators;

/// <summary>
/// Validator for Bulgaria Uniform Civil Number (EGN).
/// Format: 10 digits.
/// </summary>
public partial class BulgariaNationalIdValidator : NationalIdValidatorBase
{
    private static readonly int[] Weights = { 2, 4, 8, 5, 10, 9, 7, 3, 6 };

    /// <inheritdoc/>
        public override string CountryCode => "BG";

    /// <inheritdoc/>
    protected override bool IsValidLength(string sanitized) => sanitized.Length == 10;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string sanitized)
    {
        if (!long.TryParse(sanitized, out _))
        {
            return false;
        }

        // Validate Date
        int yearPart = int.Parse(sanitized.Substring(0, 2));
        int monthPart = int.Parse(sanitized.Substring(2, 2));
        int dayPart = int.Parse(sanitized.Substring(4, 2));

        int fullYear = 0;
        int realMonth = 0;

        if (monthPart >= 1 && monthPart <= 12)
        {
            fullYear = 1900 + yearPart;
            realMonth = monthPart;
        }
        else if (monthPart >= 21 && monthPart <= 32)
        {
            fullYear = 1800 + yearPart;
            realMonth = monthPart - 20;
        }
        else if (monthPart >= 41 && monthPart <= 52)
        {
            fullYear = 2000 + yearPart;
            realMonth = monthPart - 40;
        }
        else
        {
            return false;
        }

        return DateHelper.IsValidDate(fullYear, realMonth, dayPart);
    }

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string sanitized)
    {
        int sum = 0;
        for (int i = 0; i < 9; i++)
        {
            sum += (sanitized[i] - '0') * Weights[i];
        }

        int remainder = sum % 11;
        int checkDigit = remainder == 10 ? 0 : remainder;

        return checkDigit == (sanitized[9] - '0')
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }

    /// <summary>
    /// Static validation method for Bulgarian National ID.
    /// </summary>
        public static ValidationResult ValidateStatic(string? nationalId) => new BulgariaNationalIdValidator().Validate(nationalId);
}