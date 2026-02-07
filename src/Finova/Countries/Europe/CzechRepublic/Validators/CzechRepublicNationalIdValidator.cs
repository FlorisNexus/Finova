using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.CzechRepublic.Validators;

/// <summary>
/// Validator for Czech Birth Number (Rodné číslo).
/// Format: 9 or 10 digits.
/// </summary>
public partial class CzechRepublicNationalIdValidator : NationalIdValidatorBase
{
    /// <inheritdoc/>
        public override string CountryCode => "CZ";

    /// <inheritdoc/>
    protected override bool IsValidLength(string sanitized) => sanitized.Length == 9 || sanitized.Length == 10;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string sanitized) => long.TryParse(sanitized, out _);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string sanitized)
    {
        // Before 1954: 9 digits, no checksum
        if (sanitized.Length == 9)
        {
            return ValidationResult.Success();
        }

        // 10 digits: Mod 11
        if (!long.TryParse(sanitized, out long number))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.MustContainOnlyDigits);
        }

        if (number % 11 != 0)
        {
            // Special case: remainder 10 -> check digit 0
            long first9 = long.Parse(sanitized.Substring(0, 9));
            long remainder = first9 % 11;
            int checkDigit = sanitized[9] - '0';

            if (remainder == 10)
            {
                if (checkDigit != 0)
                {
                    return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
                }
            }
            else
            {
                if (checkDigit != remainder)
                {
                    return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
                }
            }
        }

        return ValidationResult.Success();
    }

    /// <summary>
    /// Static validation method for Czech National ID.
    /// </summary>
        public static ValidationResult ValidateStatic(string? input) => new CzechRepublicNationalIdValidator().Validate(input);
}