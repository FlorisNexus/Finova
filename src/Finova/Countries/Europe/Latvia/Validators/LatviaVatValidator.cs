using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Latvia.Validators;

/// <summary>
/// Validator for Latvian VAT numbers (PVN).
/// Format: 11 digits.
/// </summary>
public partial class LatviaVatValidator : VatValidatorBase
{
    [GeneratedRegex(@"^\d{11}$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "LV";

    /// <inheritdoc/>
        public override string CountryCode => VatPrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new LatviaVatValidator().Validate(input);


    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => cleaned.Length == 11;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => VatRegex().IsMatch(cleaned);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned)
    {
        int[] weights = { 9, 1, 4, 8, 3, 10, 2, 5, 7, 6 };

        int sum = ChecksumHelper.CalculateWeightedSum(cleaned.Substring(0, 10), weights);

        int remainder = sum % 11;
        int checkDigit = 3 - remainder;
        if (checkDigit < -1)
        {
            checkDigit += 11;
        }

        if (checkDigit == -1)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidLatviaVatChecksumResultMinusOne);
        }

        int lastDigit = cleaned[10] - '0';
        return checkDigit == lastDigit
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidLatviaVatChecksum);
    }

    /// <summary>
    /// Static validation method for Latvian VAT numbers.
    /// </summary>
        public static ValidationResult ValidateVat(string? vat) => new LatviaVatValidator().Validate(vat);

    /// <summary>
    /// Gets details for a Latvian VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat) => new LatviaVatValidator().Parse(vat);
}