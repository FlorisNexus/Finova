using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.BosniaAndHerzegovina.Validators;

/// <summary>
/// Validator for Bosnia and Herzegovina VAT numbers.
/// Format: 13 digits.
/// </summary>
public partial class BosniaAndHerzegovinaVatValidator : VatValidatorBase
{
    [GeneratedRegex(@"^\d{13}$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "BA";

    // Weights repeat: 7, 6, 5, 4, 3, 2, 7, 6, 5, 4, 3, 2
    private static readonly int[] Weights = { 7, 6, 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 };

    /// <inheritdoc/>
        public override string CountryCode => VatPrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new BosniaAndHerzegovinaVatValidator().Validate(input);


    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => cleaned.Length == 13;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => VatRegex().IsMatch(cleaned);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned)
    {
        // Last digit is check digit
        int checkDigit = cleaned[12] - '0';
        string dataPart = cleaned.Substring(0, 12);

        int sum = ChecksumHelper.CalculateWeightedSum(dataPart, Weights);
        int remainder = sum % 11;

        int calculatedCheck;
        if (remainder == 1)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidBosniaAndHerzegovinaVatChecksumRemainderOne);
        }
        else if (remainder == 0)
        {
            calculatedCheck = 0;
        }
        else
        {
            calculatedCheck = 11 - remainder;
        }

        if (calculatedCheck != checkDigit)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidBosniaAndHerzegovinaVatChecksum);
        }

        return ValidationResult.Success();
    }

    /// <summary>
    /// Static validation method for Bosnia and Herzegovina VAT numbers.
    /// </summary>
    /// <param name="vat">The VAT number to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
        public static ValidationResult ValidateVat(string? vat) => new BosniaAndHerzegovinaVatValidator().Validate(vat);

    /// <summary>
    /// Gets details for a Bosnia and Herzegovina VAT number.
    /// </summary>
    /// <param name="vat">The VAT number to parse.</param>
    /// <returns>A <see cref="VatDetails"/> object or null if invalid.</returns>
    public static VatDetails? GetVatDetails(string? vat) => new BosniaAndHerzegovinaVatValidator().Parse(vat);
}