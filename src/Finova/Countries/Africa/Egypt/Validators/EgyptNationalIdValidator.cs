using System.Globalization;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Africa.Egypt.Validators;

/// <summary>
/// Validator for Egyptian National ID numbers.
/// Format: 14 digits.
/// </summary>
public partial class EgyptNationalIdValidator : NationalIdValidatorBase
{
    /// <inheritdoc/>
        public override string CountryCode => "EG";

    /// <inheritdoc/>
    protected override bool IsValidLength(string sanitized) => sanitized.Length == 14;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string sanitized)
    {
        if (!long.TryParse(sanitized, out _))
        {
            return false;
        }

        int centuryDigit = sanitized[0] - '0';
        if (centuryDigit != 2 && centuryDigit != 3)
        {
            return false;
        }

        string datePart = sanitized.Substring(1, 6);
        return DateTime.TryParseExact(datePart, "yyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
    }

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string sanitized)
    {
        int sum = 0;
        int[] weights = { 2, 7, 6, 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 };

        for (int i = 0; i < 13; i++)
        {
            sum += (sanitized[i] - '0') * weights[i];
        }

        int remainder = sum % 11;
        int checkDigit = 11 - remainder;
        if (checkDigit >= 10)
        {
            checkDigit = 0;
        }

        return checkDigit == (sanitized[13] - '0')
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }

    /// <summary>
    /// Static validation method for Egyptian National ID.
    /// </summary>
    public static ValidationResult ValidateStatic(string? idNumber) => new EgyptNationalIdValidator().Validate(idNumber);
}