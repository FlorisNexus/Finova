using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.NorthMacedonia.Validators;

/// <summary>
/// Validator for North Macedonia National Identification Number (EMBG).
/// Format: 13 digits.
/// </summary>
public partial class NorthMacedoniaNationalIdValidator : NationalIdValidatorBase
{
    private static readonly int[] Weights = { 7, 6, 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 };

    /// <inheritdoc/>
        public override string CountryCode => "MK";

    /// <inheritdoc/>
    protected override bool IsValidLength(string sanitized) => sanitized.Length == 13;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string sanitized) => long.TryParse(sanitized, out _);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string sanitized)
    {
        int sum = 0;
        for (int i = 0; i < 12; i++)
        {
            sum += (sanitized[i] - '0') * Weights[i];
        }

        int remainder = sum % 11;
        int checkDigit;

        if (remainder == 0)
        {
            checkDigit = 0;
        }
        else if (remainder == 1)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
        }
        else
        {
            checkDigit = 11 - remainder;
        }

        return checkDigit == (sanitized[12] - '0')
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }

    /// <summary>
    /// Static validation method for North Macedonian EMBG.
    /// </summary>
        public static ValidationResult ValidateStatic(string? nationalId) => new NorthMacedoniaNationalIdValidator().Validate(nationalId);
}