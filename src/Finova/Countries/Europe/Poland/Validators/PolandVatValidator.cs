using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Poland.Validators;

/// <summary>
/// Validator for Polish VAT numbers (NIP).
/// Format: 10 digits.
/// </summary>
public partial class PolandVatValidator : VatValidatorBase
{
    [GeneratedRegex(@"^\d{10}$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "PL";

    /// <inheritdoc/>
        public override string CountryCode => VatPrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new PolandVatValidator().Validate(input);


    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => cleaned.Length == 10;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => VatRegex().IsMatch(cleaned);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned)
    {
        int[] weights = { 6, 5, 7, 2, 3, 4, 5, 6, 7 };

        int sum = ChecksumHelper.CalculateWeightedSum(cleaned.Substring(0, 9), weights);

        int remainder = sum % 11;
        if (remainder == 10)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, string.Format(ValidationMessages.InvalidVatChecksum, "Poland"));
        }

        int checkDigit = cleaned[9] - '0';
        return checkDigit == remainder
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, string.Format(ValidationMessages.InvalidVatChecksum, "Poland"));
    }

    /// <summary>
    /// Static validation method for Polish VAT numbers.
    /// </summary>
        public static ValidationResult ValidateVat(string? vat) => new PolandVatValidator().Validate(vat);

    /// <summary>
    /// Gets details for a Polish VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat) => new PolandVatValidator().Parse(vat);
}