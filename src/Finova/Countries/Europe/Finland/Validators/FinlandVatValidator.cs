using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Finland.Validators;

/// <summary>
/// Validator for Finnish VAT numbers (ALV nro).
/// Format: 8 digits.
/// </summary>
public partial class FinlandVatValidator : VatValidatorBase
{
    [GeneratedRegex(@"^\d{8}$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "FI";

    /// <inheritdoc/>
        public override string CountryCode => VatPrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new FinlandVatValidator().Validate(input);


    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => cleaned.Length == 8;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => VatRegex().IsMatch(cleaned);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned)
    {
        int[] weights = { 7, 9, 10, 5, 8, 4, 2 };

        int remainder = ChecksumHelper.CalculateWeightedModulo11(cleaned.Substring(0, 7), weights);

        int checkDigit = 11 - remainder;
        if (checkDigit == 11)
        {
            checkDigit = 0;
        }

        if (checkDigit == 10)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidFinlandVatChecksumCheckDigit10);
        }

        int lastDigit = cleaned[7] - '0';
        return checkDigit == lastDigit
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidFinlandVatChecksum);
    }

    /// <summary>
    /// Static validation method for Finnish VAT numbers.
    /// </summary>
        public static ValidationResult ValidateVat(string? vat) => new FinlandVatValidator().Validate(vat);

    /// <summary>
    /// Gets details for a Finnish VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat) => new FinlandVatValidator().Parse(vat);
}