using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.CzechRepublic.Validators;

/// <summary>
/// Validator for Czech VAT numbers (DIČ).
/// Format: 8, 9, or 10 digits.
/// </summary>
public partial class CzechRepublicVatValidator : VatValidatorBase
{
    [GeneratedRegex(@"^\d{8,10}$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "CZ";

    /// <inheritdoc/>
        public override string CountryCode => VatPrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new CzechRepublicVatValidator().Validate(input);


    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => cleaned.Length >= 8 && cleaned.Length <= 10;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => VatRegex().IsMatch(cleaned);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned)
    {
        // Standard (Legal Entities): 8 digits. Weighted Mod 11 {8, 7, 6, 5, 4, 3, 2}
        if (cleaned.Length == 8)
        {
            int[] weights = { 8, 7, 6, 5, 4, 3, 2 };
            int sum = ChecksumHelper.CalculateWeightedSum(cleaned.Substring(0, 7), weights);

            int remainder = sum % 11;
            int checkDigit = 11 - remainder;
            if (checkDigit == 10)
            {
                checkDigit = 0;
            }

            if (checkDigit == 11)
            {
                checkDigit = 1;
            }

            int lastDigit = cleaned[7] - '0';
            if (checkDigit != lastDigit)
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, string.Format(ValidationMessages.InvalidVatChecksum, "Czech Republic"));
            }
        }
        // Special (Individuals): 9 or 10 digits. Divisible by 11.
        else if (cleaned.Length == 9 || cleaned.Length == 10)
        {
            if (!ChecksumHelper.IsDivisibleBy(cleaned, 11))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, string.Format(ValidationMessages.InvalidVatChecksum, "Czech Republic"));
            }
        }

        return ValidationResult.Success();
    }

    /// <summary>
    /// Static validation method for Czech VAT numbers.
    /// </summary>
    /// <param name="vat">The VAT number to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
        public static ValidationResult ValidateVat(string? vat) => new CzechRepublicVatValidator().Validate(vat);

    /// <summary>
    /// Gets details for a Czech VAT number.
    /// </summary>
    /// <param name="vat">The VAT number to parse.</param>
    /// <returns>A <see cref="VatDetails"/> object or null if invalid.</returns>
    public static VatDetails? GetVatDetails(string? vat) => new CzechRepublicVatValidator().Parse(vat);
}
