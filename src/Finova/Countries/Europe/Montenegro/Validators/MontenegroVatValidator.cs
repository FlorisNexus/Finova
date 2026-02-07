using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Montenegro.Validators;

/// <summary>
/// Validator for Montenegrin VAT numbers.
/// Format: 8 digits.
/// </summary>
public partial class MontenegroVatValidator : VatValidatorBase
{
    [GeneratedRegex(@"^\d{8}$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "ME";
    private static readonly int[] Weights = { 7, 6, 5, 4, 3, 2, 7 };

    /// <inheritdoc/>
        public override string CountryCode => VatPrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new MontenegroVatValidator().Validate(input);


    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => cleaned.Length == 8;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => VatRegex().IsMatch(cleaned);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned)
    {
        int sum = ChecksumHelper.CalculateWeightedSum(cleaned[..7], Weights);
        int remainder = sum % 11;

        if (remainder == 10)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidMontenegroVatChecksumRemainder10);
        }

        int checkDigit = 11 - remainder;
        if (checkDigit == 11)
        {
            checkDigit = 0;
        }

        if (checkDigit != (cleaned[7] - '0'))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidMontenegroVatChecksum);
        }

        return ValidationResult.Success();
    }

    /// <summary>
    /// Static validation method for Montenegrin VAT numbers.
    /// </summary>
        public static ValidationResult ValidateVat(string? vat) => new MontenegroVatValidator().Validate(vat);

    /// <summary>
    /// Gets details for a Montenegrin VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat) => new MontenegroVatValidator().Parse(vat);
}
