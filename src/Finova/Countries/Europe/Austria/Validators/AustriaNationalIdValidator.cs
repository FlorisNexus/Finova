using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Austria.Validators;

/// <summary>
/// Validator for Austrian Social Security Number (Versicherungsnummer).
/// Format: 10 digits.
/// </summary>
public partial class AustriaNationalIdValidator : NationalIdValidatorBase
{
    private static readonly int[] Weights = { 3, 7, 9, 0, 5, 8, 4, 2, 1, 6 };

    /// <inheritdoc/>
        public override string CountryCode => "AT";

    /// <inheritdoc/>
    protected override bool IsValidLength(string sanitized) => sanitized.Length == 10;

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

        int remainder = sum % 11;
        if (remainder == 10)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
        }

        int checkDigit = sanitized[3] - '0';
        return checkDigit == remainder
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }

    /// <summary>
    /// Static validation method for Austrian Social Security Number.
    /// </summary>
        public static ValidationResult ValidateStatic(string? input) => new AustriaNationalIdValidator().Validate(input);
}