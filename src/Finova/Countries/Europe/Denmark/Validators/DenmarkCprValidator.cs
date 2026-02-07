using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Denmark.Validators;

/// <summary>
/// Validator for Danish Personal Identification Number (CPR).
/// Format: 10 digits.
/// </summary>
public partial class DenmarkCprValidator : NationalIdValidatorBase
{
    private static readonly int[] Weights = { 4, 3, 2, 7, 6, 5, 4, 3, 2, 1 };

    /// <inheritdoc/>
        public override string CountryCode => "DK";

    /// <inheritdoc/>
    protected override bool IsValidLength(string sanitized) => sanitized.Length == 10;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string sanitized)
    {
        if (!long.TryParse(sanitized, out _))
        {
            return false;
        }

        // Validate Date (DDMMYY)
        int day = int.Parse(sanitized.Substring(0, 2));
        int month = int.Parse(sanitized.Substring(2, 2));

        return month >= 1 && month <= 12 && day >= 1 && day <= 31;
    }

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string sanitized)
    {
        int sum = 0;
        for (int i = 0; i < 10; i++)
        {
            sum += (sanitized[i] - '0') * Weights[i];
        }

        return sum % 11 == 0
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }

    /// <summary>
    /// Static validation method for Danish CPR.
    /// </summary>
    public static ValidationResult ValidateStatic(string? nationalId) => new DenmarkCprValidator().Validate(nationalId);
}