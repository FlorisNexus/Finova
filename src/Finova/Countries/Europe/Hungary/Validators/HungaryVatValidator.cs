using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Hungary.Validators;

/// <summary>
/// Validator for Hungarian VAT numbers (Adószám).
/// Format: 8 digits.
/// </summary>
public partial class HungaryVatValidator : VatValidatorBase
{
    [GeneratedRegex(@"^\d{8}$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "HU";

    /// <inheritdoc/>
        public override string CountryCode => VatPrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new HungaryVatValidator().Validate(input);


    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => cleaned.Length == 8;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => VatRegex().IsMatch(cleaned);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned)
    {
        // Weights: 9, 7, 3, 1, 9, 7, 3
        int[] weights = { 9, 7, 3, 1, 9, 7, 3 };

        int sum = ChecksumHelper.CalculateWeightedSum(cleaned.Substring(0, 7), weights);

        int checkDigit = 10 - (sum % 10);
        if (checkDigit == 10)
        {
            checkDigit = 0;
        }

        int lastDigit = cleaned[7] - '0';
        return checkDigit == lastDigit
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidHungaryVatChecksum);
    }

    /// <summary>
    /// Static validation method for Hungarian VAT numbers.
    /// </summary>
        public static ValidationResult ValidateVat(string? vat) => new HungaryVatValidator().Validate(vat);

    /// <summary>
    /// Gets details for a Hungarian VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat) => new HungaryVatValidator().Parse(vat);
}