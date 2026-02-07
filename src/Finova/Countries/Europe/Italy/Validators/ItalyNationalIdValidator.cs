using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Italy.Validators;

/// <summary>
/// Validator for Italian National ID (Codice Fiscale).
/// Format: 16 characters.
/// </summary>
public partial class ItalyNationalIdValidator : NationalIdValidatorBase
{
    private static readonly int[] OddDigitValues = { 1, 0, 5, 7, 9, 13, 15, 17, 19, 21 };
    private static readonly int[] OddLetterValues = {
        1, 0, 5, 7, 9, 13, 15, 17, 19, 21, // A-J
        2, 4, 18, 20, 11, 3, 6, 8, 12, 14, // K-T
        16, 10, 22, 25, 24, 23             // U-Z
    };

    /// <inheritdoc/>
        public override string CountryCode => "IT";

    /// <inheritdoc/>
    protected override bool IsValidLength(string sanitized) => sanitized.Length == 16;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string sanitized)
    {
        // First 15 are alphanumeric, last is letter.
        if (!char.IsLetter(sanitized[15]))
        {
            return false;
        }

        foreach (char c in sanitized.Substring(0, 15))
        {
            if (!char.IsLetterOrDigit(c))
            {
                return false;
            }
        }
        return true;
    }

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string sanitized)
    {
        char checkDigit = sanitized[15];
        int total = 0;

        for (int i = 0; i < 15; i++)
        {
            char c = sanitized[i];
            bool isOddPosition = (i + 1) % 2 != 0; // 1-based index

            if (isOddPosition)
            {
                if (char.IsDigit(c))
                {
                    total += OddDigitValues[c - '0'];
                }
                else
                {
                    total += OddLetterValues[c - 'A'];
                }
            }
            else
            {
                if (char.IsDigit(c))
                {
                    total += c - '0';
                }
                else
                {
                    total += c - 'A';
                }
            }
        }

        int remainder = total % 26;
        char expectedCheckDigit = (char)('A' + remainder);

        return checkDigit == expectedCheckDigit
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }

    /// <summary>
    /// Static validation method for Italian Codice Fiscale.
    /// </summary>
        public static ValidationResult ValidateStatic(string? codiceFiscale) => new ItalyNationalIdValidator().Validate(codiceFiscale);
}