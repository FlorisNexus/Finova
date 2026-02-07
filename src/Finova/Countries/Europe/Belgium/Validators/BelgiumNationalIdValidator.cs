using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Belgium.Validators;

/// <summary>
/// Validator for Belgian National Number (Numéro de registre national / Rijksregisternummer).
/// Format: 11 digits.
/// </summary>
public partial class BelgiumNationalIdValidator : NationalIdValidatorBase
{
    /// <inheritdoc/>
        public override string CountryCode => "BE";

    /// <inheritdoc/>
    protected override bool IsValidLength(string sanitized) => sanitized.Length == 11;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string sanitized)
    {
        if (!long.TryParse(sanitized, out _))
        {
            return false;
        }

        // Validate the date part (YYMMDD), accounting for Bis/Ter numbers
        return IsValidBelgianDate(sanitized[..6]);
    }

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string sanitized)
    {
        long number = long.Parse(sanitized[..9]);
        long checksum = long.Parse(sanitized[9..]);

        // Standard Check: Individuals born before 2000
        // Formula: 97 - (Number % 97)
        long calculatedChecksum = 97 - (number % 97);
        if (calculatedChecksum == checksum)
        {
            return ValidationResult.Success();
        }

        // 2000+ Check: Individuals born in or after 2000
        long number2000 = number + 2000000000L;
        long calculatedChecksum2000 = 97 - (number2000 % 97);

        return calculatedChecksum2000 == checksum
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }

    private static bool IsValidBelgianDate(string datePart)
    {
        int month = int.Parse(datePart[2..4]);
        int day = int.Parse(datePart[4..]);

        if (month > 40)
        {
            month -= 40;
        }
        else if (month > 20)
        {
            month -= 20;
        }

        if (month > 12)
        {
            return false;
        }

        if (day > 31)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Static validation method for Belgian National ID.
    /// </summary>
        public static ValidationResult ValidateStatic(string? input) => new BelgiumNationalIdValidator().Validate(input);
}