using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Belarus.Validators;

/// <summary>
/// Validator for Belarusian VAT numbers (УНП).
/// Format: 9 digits.
/// </summary>
public partial class BelarusVatValidator : VatValidatorBase
{
    private const string CountryCodePrefix = "BY";
    private const int VatLength = 9;

    [GeneratedRegex(@"^\d{9}$")]
    private static partial Regex VatRegex();

    /// <inheritdoc/>
        public override string CountryCode => CountryCodePrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new BelarusVatValidator().Validate(input);


    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => cleaned.Length == VatLength;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => VatRegex().IsMatch(cleaned);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned)
    {
        int[] weights = { 29, 23, 19, 17, 13, 7, 5, 3 };
        string digits = cleaned[..8];
        int checkDigit = cleaned[8] - '0';

        int sum = ChecksumHelper.CalculateWeightedSum(digits, weights);
        int calculated = sum % 11;

        if (calculated == 10)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.BelarusVatInvalidChecksumMod11);
        }

        if (calculated != checkDigit)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
        }

        return ValidationResult.Success();
    }

    /// <summary>
    /// Static validation method for Belarusian VAT numbers.
    /// </summary>
        public static ValidationResult ValidateVat(string? vat) => new BelarusVatValidator().Validate(vat);

    /// <summary>
    /// Gets details for a Belarusian VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat) => new BelarusVatValidator().Parse(vat);
}