using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Lithuania.Validators;

/// <summary>
/// Validator for Lithuanian VAT numbers (PVM mokėtojo kodas).
/// Format: 9 or 12 digits.
/// </summary>
public partial class LithuaniaVatValidator : VatValidatorBase
{
    [GeneratedRegex(@"^(\d{9}|\d{12})$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "LT";

    /// <inheritdoc/>
        public override string CountryCode => VatPrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new LithuaniaVatValidator().Validate(input);


    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => cleaned.Length == 9 || cleaned.Length == 12;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => VatRegex().IsMatch(cleaned);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned)
    {
        if (cleaned.Length == 9)
        {
            int[] weights1 = { 1, 2, 3, 4, 5, 6, 7, 8 };
            int sum = ChecksumHelper.CalculateWeightedSum(cleaned.Substring(0, 8), weights1);

            int remainder = sum % 11;
            if (remainder == 10)
            {
                int[] weights2 = { 3, 4, 5, 6, 7, 8, 9, 1 };
                sum = ChecksumHelper.CalculateWeightedSum(cleaned.Substring(0, 8), weights2);
                remainder = sum % 11;
                if (remainder == 10)
                {
                    remainder = 0;
                }
            }

            int checkDigit = cleaned[8] - '0';
            if (remainder != checkDigit)
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidLithuaniaVatChecksum);
            }
        }
        else if (cleaned.Length == 12)
        {
            int[] weights1 = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 1, 2 };
            int sum = ChecksumHelper.CalculateWeightedSum(cleaned.Substring(0, 11), weights1);

            int remainder = sum % 11;
            if (remainder == 10)
            {
                int[] weights2 = { 3, 4, 5, 6, 7, 8, 9, 1, 2, 3, 4 };
                sum = ChecksumHelper.CalculateWeightedSum(cleaned.Substring(0, 11), weights2);
                remainder = sum % 11;
                if (remainder == 10)
                {
                    remainder = 0;
                }
            }

            int checkDigit = cleaned[11] - '0';
            if (remainder != checkDigit)
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidLithuaniaVatChecksum);
            }
        }

        return ValidationResult.Success();
    }

    /// <summary>
    /// Static validation method for Lithuanian VAT numbers.
    /// </summary>
        public static ValidationResult ValidateVat(string? vat) => new LithuaniaVatValidator().Validate(vat);

    /// <summary>
    /// Gets details for a Lithuanian VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat) => new LithuaniaVatValidator().Parse(vat);
}