using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Romania.Validators;

/// <summary>
/// Validator for Romanian VAT numbers (CIF).
/// Format: 2 to 10 digits.
/// </summary>
public partial class RomaniaVatValidator : VatValidatorBase
{
    [GeneratedRegex(@"^\d{2,10}$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "RO";
    // Weights for Romanian CIF (applied to the first 9 digits, padded with 0 if needed)
    private static readonly int[] Weights = { 7, 5, 3, 2, 1, 7, 5, 3, 2 };

    /// <inheritdoc/>
        public override string CountryCode => VatPrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new RomaniaVatValidator().Validate(input);


    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => cleaned.Length >= 2 && cleaned.Length <= 10;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => VatRegex().IsMatch(cleaned);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned)
    {
        string padded = cleaned.PadLeft(10, '0');

        string dataPart = padded[..9];
        int checkDigit = padded[9] - '0';

        int sum = ChecksumHelper.CalculateWeightedSum(dataPart, Weights);

        int calculated = sum * 10 % 11;
        if (calculated == 10)
        {
            calculated = 0;
        }

        return calculated == checkDigit
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidRomaniaVatChecksum);
    }

    /// <summary>
    /// Static validation method for Romanian VAT numbers.
    /// </summary>
        public static ValidationResult ValidateVat(string? vat) => new RomaniaVatValidator().Validate(vat);

    /// <summary>
    /// Gets details for a Romanian VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat) => new RomaniaVatValidator().Parse(vat);
}
