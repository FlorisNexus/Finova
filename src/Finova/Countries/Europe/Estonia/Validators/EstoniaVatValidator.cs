using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Estonia.Validators;

/// <summary>
/// Validator for Estonian VAT numbers (KMKR).
/// Format: 9 digits.
/// </summary>
public partial class EstoniaVatValidator : VatValidatorBase
{
    [GeneratedRegex(@"^\d{9}$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "EE";

    /// <inheritdoc/>
        public override string CountryCode => VatPrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new EstoniaVatValidator().Validate(input);


    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => cleaned.Length == 9;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => VatRegex().IsMatch(cleaned);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned)
    {
        int[] weights = { 3, 7, 1, 3, 7, 1, 3, 7 };

        int sum = ChecksumHelper.CalculateWeightedSum(cleaned.Substring(0, 8), weights);

        int checkDigit = 10 - (sum % 10);
        if (checkDigit == 10)
        {
            checkDigit = 0;
        }

        int lastDigit = cleaned[8] - '0';
        return checkDigit == lastDigit
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidEstoniaVatChecksum);
    }

    /// <summary>
    /// Static validation method for Estonian VAT numbers.
    /// </summary>
        public static ValidationResult ValidateVat(string? vat) => new EstoniaVatValidator().Validate(vat);

    /// <summary>
    /// Gets details for an Estonian VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat) => new EstoniaVatValidator().Parse(vat);
}