using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Spain.Validators;

/// <summary>
/// Validator for Spanish VAT numbers (NIF/NIE/CIF).
/// Format: 9 characters.
/// </summary>
public partial class SpainVatValidator : VatValidatorBase
{
    // Spain VAT (NIF) format: 9 characters.
    // 1. 8 digits + 1 letter (DNI)
    // 2. 1 letter + 7 digits + 1 letter (NIE)
    // 3. 1 letter + 8 digits (CIF)
    [GeneratedRegex(@"^[A-Z0-9]{9}$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "ES";

    /// <inheritdoc/>
        public override string CountryCode => VatPrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new SpainVatValidator().Validate(input);


    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => cleaned.Length == 9;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => VatRegex().IsMatch(cleaned);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned)
    {
        char firstChar = cleaned[0];
        char lastChar = cleaned[8];
        string digits = cleaned.Substring(1, 7);

        if (char.IsDigit(firstChar))
        {
            // DNI (National entities)
            string numberPart = cleaned.Substring(0, 8);
            if (!long.TryParse(numberPart, out long number))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, string.Format(ValidationMessages.InvalidVatFormat, "Spain DNI"));
            }

            string controlChars = "TRWAGMYFPDXBNJZSQVHLCKE";
            char expectedChar = controlChars[(int)(number % 23)];

            if (lastChar != expectedChar)
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, string.Format(ValidationMessages.InvalidVatChecksum, "Spain DNI"));
            }
        }
        else if (firstChar == 'X' || firstChar == 'Y' || firstChar == 'Z')
        {
            // NIE (Foreigners)
            string prefix = firstChar == 'X' ? "0" : firstChar == 'Y' ? "1" : "2";
            string numberPart = prefix + cleaned.Substring(1, 7);

            if (!long.TryParse(numberPart, out long number))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, string.Format(ValidationMessages.InvalidVatFormat, "Spain NIE"));
            }

            string controlChars = "TRWAGMYFPDXBNJZSQVHLCKE";
            char expectedChar = controlChars[(int)(number % 23)];

            if (lastChar != expectedChar)
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, string.Format(ValidationMessages.InvalidVatChecksum, "Spain NIE"));
            }
        }
        else
        {
            // CIF (Juridical entities)
            int sum = 0;
            for (int i = 0; i < 7; i++)
            {
                int digit = digits[i] - '0';
                if (i % 2 == 0)
                {
                    int doubled = digit * 2;
                    sum += (doubled / 10) + (doubled % 10);
                }
                else
                {
                    sum += digit;
                }
            }

            int controlDigit = (10 - (sum % 10)) % 10;
            char[] controlLetters = { 'J', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I' };
            char expectedLetter = controlLetters[controlDigit];

            bool validDigit = char.IsDigit(lastChar) && (lastChar - '0' == controlDigit);
            bool validLetter = lastChar == expectedLetter;

            if (!validDigit && !validLetter)
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, string.Format(ValidationMessages.InvalidVatChecksum, "Spain CIF"));
            }
        }

        return ValidationResult.Success();
    }

    /// <summary>
    /// Static validation method for Spanish VAT numbers.
    /// </summary>
        public static ValidationResult ValidateVat(string? vat) => new SpainVatValidator().Validate(vat);

    /// <summary>
    /// Gets details for a Spanish VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat) => new SpainVatValidator().Parse(vat);
}