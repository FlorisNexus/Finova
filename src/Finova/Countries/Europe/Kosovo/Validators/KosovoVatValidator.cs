using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Kosovo.Validators;

/// <summary>
/// Validator for Kosovo VAT numbers (Fiscal Number).
/// Format: 9 digits.
/// </summary>
public partial class KosovoVatValidator : VatValidatorBase
{
    [GeneratedRegex(@"^\d{9}$")]
    private static partial Regex VatRegex();

    /// <inheritdoc/>
        public override string CountryCode => "XK";
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new KosovoVatValidator().Validate(input);


    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => cleaned.Length == 9;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => VatRegex().IsMatch(cleaned);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned)
    {
        // Weights: [9, 8, 7, 6, 5, 4, 3, 2] applied to first 8 digits.
        int[] weights = { 9, 8, 7, 6, 5, 4, 3, 2 };
        int sum = 0;

        for (int i = 0; i < 8; i++)
        {
            sum += (cleaned[i] - '0') * weights[i];
        }

        int remainder = sum % 11;
        int checkDigit = remainder == 10 ? 0 : remainder;

        int lastDigit = cleaned[8] - '0';
        return checkDigit == lastDigit
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidCheckDigit, ValidationMessages.InvalidChecksum);
    }

    /// <summary>
    /// Static validation method for Kosovo VAT numbers.
    /// </summary>
        public static ValidationResult ValidateVat(string? vat) => new KosovoVatValidator().Validate(vat);
}
