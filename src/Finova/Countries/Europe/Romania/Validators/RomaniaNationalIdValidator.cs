using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Romania.Validators;

/// <summary>
/// Validator for Romanian Personal Numeric Code (CNP).
/// Format: 13 digits.
/// </summary>
public partial class RomaniaNationalIdValidator : NationalIdValidatorBase
{
    private static readonly int[] Weights = { 2, 7, 9, 1, 4, 6, 3, 5, 8, 2, 7, 9 };

    /// <inheritdoc/>
        public override string CountryCode => "RO";

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
        int checkDigit = remainder == 10 ? 1 : remainder;

        return checkDigit == (sanitized[12] - '0')
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }

    /// <summary>
    /// Static validation method for Romanian CNP.
    /// </summary>
        public static ValidationResult ValidateStatic(string? input) => new RomaniaNationalIdValidator().Validate(input);
}