using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Poland.Validators;

/// <summary>
/// Validator for Polish National Identification Number (PESEL).
/// Format: 11 digits.
/// </summary>
public partial class PolandNationalIdValidator : NationalIdValidatorBase
{
    private static readonly int[] Weights = { 1, 3, 7, 9, 1, 3, 7, 9, 1, 3 };

    /// <inheritdoc/>
        public override string CountryCode => "PL";

    /// <inheritdoc/>
    protected override bool IsValidLength(string sanitized) => sanitized.Length == 11;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string sanitized) => long.TryParse(sanitized, out _);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string sanitized)
    {
        int sum = 0;
        for (int i = 0; i < 10; i++)
        {
            sum += (sanitized[i] - '0') * Weights[i];
        }

        int remainder = sum % 10;
        int checkDigit = remainder == 0 ? 0 : 10 - remainder;

        return checkDigit == (sanitized[10] - '0')
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }

    /// <summary>
    /// Static validation method for Polish PESEL.
    /// </summary>
        public static ValidationResult ValidateStatic(string? input) => new PolandNationalIdValidator().Validate(input);
}