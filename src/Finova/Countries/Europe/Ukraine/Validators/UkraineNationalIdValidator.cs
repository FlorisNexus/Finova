using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Ukraine.Validators;

/// <summary>
/// Validator for Ukraine National Identification Number (RNTRC).
/// Format: 10 digits.
/// </summary>
public partial class UkraineNationalIdValidator : NationalIdValidatorBase
{
    private static readonly int[] Weights = { -1, 5, 7, 9, 4, 6, 10, 5, 7 };

    /// <inheritdoc/>
        public override string CountryCode => "UA";

    /// <inheritdoc/>
    protected override bool IsValidLength(string sanitized) => sanitized.Length == 10;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string sanitized) => long.TryParse(sanitized, out _);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string sanitized)
    {
        int sum = 0;
        for (int i = 0; i < 9; i++)
        {
            sum += (sanitized[i] - '0') * Weights[i];
        }

        int remainder = sum % 11;
        int checkDigit = (remainder == 10) ? 0 : remainder;

        return checkDigit == (sanitized[9] - '0')
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }

    /// <summary>
    /// Static validation method for Ukrainian RNTRC.
    /// </summary>
        public static ValidationResult ValidateStatic(string? nationalId) => new UkraineNationalIdValidator().Validate(nationalId);
}