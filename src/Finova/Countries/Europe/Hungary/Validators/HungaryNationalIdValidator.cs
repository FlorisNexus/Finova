using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Hungary.Validators;

/// <summary>
/// Validator for Hungarian Social Security Number (TAJ szám).
/// Format: 9 digits.
/// </summary>
public partial class HungaryNationalIdValidator : NationalIdValidatorBase
{
    private static readonly int[] Weights = { 3, 7, 3, 7, 3, 7, 3, 7 };

    /// <inheritdoc/>
        public override string CountryCode => "HU";

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

        int remainder = sum % 10;
        return remainder == (sanitized[8] - '0')
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }

    /// <summary>
    /// Static validation method for Hungarian National ID.
    /// </summary>
        public static ValidationResult ValidateStatic(string? input) => new HungaryNationalIdValidator().Validate(input);
}