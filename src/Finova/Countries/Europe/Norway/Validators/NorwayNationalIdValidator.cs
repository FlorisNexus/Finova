using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Norway.Validators;

/// <summary>
/// Validator for Norway National Identity Number (Fødselsnummer).
/// Format: 11 digits.
/// </summary>
public partial class NorwayNationalIdValidator : NationalIdValidatorBase
{
    private static readonly int[] Weights1 = { 3, 7, 6, 1, 8, 9, 4, 5, 2 };
    private static readonly int[] Weights2 = { 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 };

    /// <inheritdoc/>
        public override string CountryCode => "NO";

    /// <inheritdoc/>
    protected override bool IsValidLength(string sanitized) => sanitized.Length == 11;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string sanitized) => long.TryParse(sanitized, out _);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string sanitized)
    {
        // Checksum 1 (d10)
        int sum1 = 0;
        for (int i = 0; i < 9; i++)
        {
            sum1 += (sanitized[i] - '0') * Weights1[i];
        }
        int remainder1 = sum1 % 11;
        int checkDigit1 = (remainder1 == 0) ? 0 : 11 - remainder1;
        if (checkDigit1 == 10 || checkDigit1 != (sanitized[9] - '0'))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
        }

        // Checksum 2 (d11)
        int sum2 = 0;
        for (int i = 0; i < 10; i++)
        {
            sum2 += (sanitized[i] - '0') * Weights2[i];
        }
        int remainder2 = sum2 % 11;
        int checkDigit2 = (remainder2 == 0) ? 0 : 11 - remainder2;
        if (checkDigit2 == 10 || checkDigit2 != (sanitized[10] - '0'))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
        }

        return ValidationResult.Success();
    }

    /// <summary>
    /// Static validation method for Norwegian Fødselsnummer.
    /// </summary>
        public static ValidationResult ValidateStatic(string? nationalId) => new NorwayNationalIdValidator().Validate(nationalId);
}