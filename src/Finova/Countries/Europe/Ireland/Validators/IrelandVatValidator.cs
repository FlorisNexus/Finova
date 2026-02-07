using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Ireland.Validators;

/// <summary>
/// Validator for Irish VAT numbers.
/// Format: pre-2013 and post-2013 formats supported.
/// </summary>
public partial class IrelandVatValidator : VatValidatorBase
{
    [GeneratedRegex(@"^(\d{7}[A-W][A-I]?|\d[A-Z+*]\d{5}[A-W])$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "IE";
    private static readonly int[] Weights = [8, 7, 6, 5, 4, 3, 2];

    /// <inheritdoc/>
        public override string CountryCode => VatPrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new IrelandVatValidator().Validate(input);


    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => cleaned.Length == 8 || cleaned.Length == 9;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => VatRegex().IsMatch(cleaned);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned)
    {
        int sum;
        char checkChar;

        if (char.IsLetter(cleaned[1]) || cleaned[1] == '+' || cleaned[1] == '*')
        {
            string numberPart = "0" + cleaned.Substring(2, 5) + cleaned[0];
            int extraSum = 9 * GetValue(cleaned[1]);

            sum = ChecksumHelper.CalculateWeightedSum(numberPart, Weights) + extraSum;
            checkChar = cleaned[7];
        }
        else
        {
            string numberPart = cleaned.Substring(0, 7);
            sum = ChecksumHelper.CalculateWeightedSum(numberPart, Weights);

            if (cleaned.Length == 9)
            {
                sum += 9 * GetValue(cleaned[8]);
                checkChar = cleaned[7];
            }
            else
            {
                checkChar = cleaned[7];
            }
        }

        int remainder = sum % 23;
        char expectedChar = remainder == 0 ? 'W' : (char)('A' + remainder - 1);

        if (checkChar != expectedChar)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidIrelandVatChecksum);
        }

        return ValidationResult.Success();
    }

    private static int GetValue(char c)
    {
        if (char.IsDigit(c))
        {
            return c - '0';
        }

        if (c == '+' || c == '*')
        {
            return 0;
        }

        return c - 'A' + 1;
    }

    /// <summary>
    /// Static validation method for Irish VAT numbers.
    /// </summary>
        public static ValidationResult ValidateVat(string? vat) => new IrelandVatValidator().Validate(vat);

    /// <summary>
    /// Gets details for an Irish VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat) => new IrelandVatValidator().Parse(vat);
}
