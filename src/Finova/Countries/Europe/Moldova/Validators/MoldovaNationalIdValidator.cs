using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Moldova.Validators;

/// <summary>
/// Validator for Moldova National Identification Number (IDNP).
/// Format: 13 digits.
/// </summary>
public partial class MoldovaNationalIdValidator : NationalIdValidatorBase
{
    private static readonly int[] Weights = { 7, 3, 1, 7, 3, 1, 7, 3, 1, 7, 3, 1 };

    /// <inheritdoc/>
        public override string CountryCode => "MD";

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

        int remainder = sum % 10;
        return remainder == (sanitized[12] - '0')
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }

    /// <summary>
    /// Static validation method for Moldovan IDNP.
    /// </summary>
        public static ValidationResult ValidateStatic(string? nationalId) => new MoldovaNationalIdValidator().Validate(nationalId);
}