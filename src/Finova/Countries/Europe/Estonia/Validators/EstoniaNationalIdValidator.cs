using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Estonia.Validators;

/// <summary>
/// Validator for Estonian Personal Identification Code (Isikukood).
/// Format: 11 digits.
/// </summary>
public partial class EstoniaNationalIdValidator : NationalIdValidatorBase
{
    private static readonly int[] Weights1 = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 1 };
    private static readonly int[] Weights2 = { 3, 4, 5, 6, 7, 8, 9, 1, 2, 3 };

    /// <inheritdoc/>
        public override string CountryCode => "EE";

    /// <inheritdoc/>
    protected override bool IsValidLength(string sanitized) => sanitized.Length == 11;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string sanitized)
    {
        if (!long.TryParse(sanitized, out _))
        {
            return false;
        }

        // Validate Gender/Century (1st digit)
        int genderCentury = sanitized[0] - '0';
        if (genderCentury < 1 || genderCentury > 6)
        {
            return false;
        }

        // Validate Date
        int year = (sanitized[1] - '0') * 10 + (sanitized[2] - '0');
        int month = (sanitized[3] - '0') * 10 + (sanitized[4] - '0');
        int day = (sanitized[5] - '0') * 10 + (sanitized[6] - '0');

        int century = genderCentury switch
        {
            1 or 2 => 1800,
            3 or 4 => 1900,
            5 or 6 => 2000,
            _ => 0
        };

        return DateHelper.IsValidDate(century + year, month, day);
    }

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string sanitized)
    {
        int remainder = ChecksumHelper.CalculateWeightedModulo11(sanitized.Substring(0, 10), Weights1);
        int checkDigit;

        if (remainder == 10)
        {
            remainder = ChecksumHelper.CalculateWeightedModulo11(sanitized.Substring(0, 10), Weights2);
            checkDigit = remainder == 10 ? 0 : remainder;
        }
        else
        {
            checkDigit = remainder;
        }

        return checkDigit == (sanitized[10] - '0')
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }

    /// <summary>
    /// Static validation method for Estonian National ID.
    /// </summary>
        public static ValidationResult ValidateStatic(string? nationalId) => new EstoniaNationalIdValidator().Validate(nationalId);
}