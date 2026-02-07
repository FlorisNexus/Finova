using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Bulgaria.Validators;

/// <summary>
/// Validator for Bulgarian VAT numbers.
/// Format: 9 or 10 digits.
/// </summary>
public partial class BulgariaVatValidator : VatValidatorBase
{
    [GeneratedRegex(@"^\d{9,10}$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "BG";

    /// <inheritdoc/>
        public override string CountryCode => VatPrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new BulgariaVatValidator().Validate(input);


    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => cleaned.Length == 9 || cleaned.Length == 10;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => VatRegex().IsMatch(cleaned);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned)
    {
        bool isValid;
        if (cleaned.Length == 9)
        {
            isValid = Validate9DigitVat(cleaned);
        }
        else
        {
            // 10 digits: Try EGN (Physical) first, then PNF (Foreigner)
            isValid = Validate10DigitVat(cleaned);
        }

        return isValid
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidBulgariaVatChecksum);
    }

    private static bool Validate9DigitVat(string vat)
    {
        int[] weights1 = { 1, 2, 3, 4, 5, 6, 7, 8 };
        int sum = 0;

        for (int i = 0; i < 8; i++)
        {
            sum += (vat[i] - '0') * weights1[i];
        }

        int remainder = sum % 11;

        if (remainder != 10)
        {
            return remainder == (vat[8] - '0');
        }

        int[] weights2 = { 3, 4, 5, 6, 7, 8, 9, 10 };
        sum = 0;
        for (int i = 0; i < 8; i++)
        {
            sum += (vat[i] - '0') * weights2[i];
        }

        remainder = sum % 11;
        if (remainder == 10)
        {
            remainder = 0;
        }

        return remainder == (vat[8] - '0');
    }

    private static bool Validate10DigitVat(string vat)
    {
        int[] weightsEgn = { 2, 4, 8, 5, 10, 9, 7, 3, 6 };
        int sum = 0;
        for (int i = 0; i < 9; i++)
        {
            sum += (vat[i] - '0') * weightsEgn[i];
        }
        int remainder = sum % 11;
        if (remainder == 10)
        {
            remainder = 0;
        }

        if (remainder == (vat[9] - '0'))
        {
            return true;
        }

        int[] weightsPnf = { 21, 19, 17, 13, 11, 9, 7, 3, 1 };
        sum = 0;
        for (int i = 0; i < 9; i++)
        {
            sum += (vat[i] - '0') * weightsPnf[i];
        }
        remainder = sum % 10;

        return remainder == (vat[9] - '0');
    }

    /// <summary>
    /// Static validation method for Bulgarian VAT numbers.
    /// </summary>
    /// <param name="vat">The VAT number to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
        public static ValidationResult ValidateVat(string? vat) => new BulgariaVatValidator().Validate(vat);

    /// <summary>
    /// Gets details for a Bulgarian VAT number.
    /// </summary>
    /// <param name="vat">The VAT number to parse.</param>
    /// <returns>A <see cref="VatDetails"/> object or null if invalid.</returns>
    public static VatDetails? GetVatDetails(string? vat) => new BulgariaVatValidator().Parse(vat);
}