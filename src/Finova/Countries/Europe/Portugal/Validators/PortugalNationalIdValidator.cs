using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Portugal.Validators;

/// <summary>
/// Validator for Portuguese Tax Identification Number (NIF).
/// Format: 9 digits.
/// </summary>
public partial class PortugalNationalIdValidator : NationalIdValidatorBase
{
    private static readonly int[] Weights = { 9, 8, 7, 6, 5, 4, 3, 2 };

    /// <inheritdoc/>
        public override string CountryCode => "PT";

    /// <inheritdoc/>
    protected override bool IsValidLength(string sanitized) => sanitized.Length == 9;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string sanitized) => long.TryParse(sanitized, out _);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string sanitized)
    {
        int sum = 0;
        for (int i = 0; i < 8; i++)
        {
            sum += (sanitized[i] - '0') * Weights[i];
        }

        int remainder = sum % 11;
        int checkDigit = remainder < 2 ? 0 : 11 - remainder;

        return checkDigit == (sanitized[8] - '0')
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }

    /// <summary>
    /// Static validation method for Portuguese NIF.
    /// </summary>
        public static ValidationResult ValidateStatic(string? input) => new PortugalNationalIdValidator().Validate(input);
}