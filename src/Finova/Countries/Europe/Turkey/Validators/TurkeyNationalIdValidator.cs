using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Turkey.Validators;

/// <summary>
/// Validator for Turkey National Identification Number (T.C. Kimlik No).
/// Format: 11 digits.
/// </summary>
public partial class TurkeyNationalIdValidator : NationalIdValidatorBase
{
    /// <inheritdoc/>
        public override string CountryCode => "TR";

    /// <inheritdoc/>
    protected override bool IsValidLength(string sanitized) => sanitized.Length == 11;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string sanitized)
    {
        return sanitized[0] != '0' && long.TryParse(sanitized, out _);
    }

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string sanitized)
    {
        int[] digits = new int[11];
        for (int i = 0; i < 11; i++)
        {
            digits[i] = sanitized[i] - '0';
        }

        // d10 calculation
        int sumOdd = digits[0] + digits[2] + digits[4] + digits[6] + digits[8];
        int sumEven = digits[1] + digits[3] + digits[5] + digits[7];

        int d10 = ((sumOdd * 7) - sumEven) % 10;
        if (d10 < 0)
        {
            d10 += 10;
        }

        if (digits[9] != d10)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
        }

        // d11 calculation
        int sumAll = 0;
        for (int i = 0; i < 10; i++)
        {
            sumAll += digits[i];
        }

        int d11 = sumAll % 10;

        return digits[10] == d11
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }

    /// <summary>
    /// Static validation method for Turkish T.C. Kimlik No.
    /// </summary>
        public static ValidationResult ValidateStatic(string? nationalId) => new TurkeyNationalIdValidator().Validate(nationalId);
}