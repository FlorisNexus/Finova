using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Asia.SouthKorea.Validators;

/// <summary>
/// Validator for South Korean RRN (Resident Registration Number / 주민등록번호).
/// Format: 13 digits (YYMMDD-GNNNNNN) where G is the gender/century digit.
/// </summary>
public partial class SouthKoreaNationalIdValidator : NationalIdValidatorBase
{
    [GeneratedRegex(@"^\d{13}$")]
    private static partial Regex FormatRegex();

    /// <inheritdoc/>
    public override string CountryCode => "KR";

    /// <inheritdoc/>
    protected override bool IsValidLength(string sanitized) => sanitized.Length == 13;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string sanitized)
    {
        if (!FormatRegex().IsMatch(sanitized))
        {
            return false;
        }

        // Gender/century digit must be 1-4 (1900s male/female, 2000s male/female)
        int genderDigit = sanitized[6] - '0';
        if (genderDigit < 1 || genderDigit > 4)
        {
            return false;
        }

        // Basic date validation (YYMMDD)
        int month = int.Parse(sanitized.Substring(2, 2));
        int day = int.Parse(sanitized.Substring(4, 2));

        return month >= 1 && month <= 12 && day >= 1 && day <= 31;
    }

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string sanitized)
    {
        int[] weights = [2, 3, 4, 5, 6, 7, 8, 9, 2, 3, 4, 5];
        int sum = 0;

        for (int i = 0; i < 12; i++)
        {
            sum += (sanitized[i] - '0') * weights[i];
        }

        int checkDigit = (11 - (sum % 11)) % 10;

        return checkDigit == (sanitized[12] - '0')
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }

    /// <summary>
    /// Static validation method for South Korean RRN.
    /// </summary>
    public static ValidationResult ValidateStatic(string? number) => new SouthKoreaNationalIdValidator().Validate(number);
}
