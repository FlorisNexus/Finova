using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.FaroeIslands.Validators;

/// <summary>
/// Validator for Faroe Islands Personal Identification Number (P-tal).
/// Format: 9 digits.
/// </summary>
public partial class FaroeIslandsNationalIdValidator : NationalIdValidatorBase
{
    private static readonly int[] Weights = { 3, 2, 7, 6, 5, 4, 3, 2 };

    /// <inheritdoc/>
        public override string CountryCode => "FO";

    /// <inheritdoc/>
    protected override bool IsValidLength(string sanitized) => sanitized.Length == 9;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string sanitized)
    {
        if (!long.TryParse(sanitized, out _))
        {
            return false;
        }

        // Date Validation (DDMMYY)
        int day = int.Parse(sanitized.Substring(0, 2));
        int month = int.Parse(sanitized.Substring(2, 2));

        return month >= 1 && month <= 12 && day >= 1 && day <= 31;
    }

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string sanitized)
    {
        int sum = 0;
        for (int i = 0; i < 8; i++)
        {
            sum += (sanitized[i] - '0') * Weights[i];
        }

        int remainder = sum % 11;
        int checkDigit = (remainder == 0) ? 0 : 11 - remainder;

        if (checkDigit == 10)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
        }

        return checkDigit == (sanitized[8] - '0')
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }

    /// <summary>
    /// Static validation method for Faroe Islands P-tal.
    /// </summary>
        public static ValidationResult ValidateStatic(string? nationalId) => new FaroeIslandsNationalIdValidator().Validate(nationalId);
}