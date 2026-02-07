using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Slovakia.Validators;

/// <summary>
/// Validator for Slovak VAT numbers (IČ DPH).
/// Format: 10 digits.
/// </summary>
public partial class SlovakiaVatValidator : VatValidatorBase
{
    [GeneratedRegex(@"^\d{10}$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "SK";

    /// <inheritdoc/>
        public override string CountryCode => VatPrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new SlovakiaVatValidator().Validate(input);


    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => cleaned.Length == 10;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => VatRegex().IsMatch(cleaned);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned)
    {
        if (!long.TryParse(cleaned, out long numericValue))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidSlovakiaVatFormatNonNumeric);
        }

        return numericValue % 11 == 0
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidSlovakiaVatChecksum);
    }

    /// <summary>
    /// Static validation method for Slovak VAT numbers.
    /// </summary>
        public static ValidationResult ValidateVat(string? vat) => new SlovakiaVatValidator().Validate(vat);

    /// <summary>
    /// Gets details for a Slovak VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat) => new SlovakiaVatValidator().Parse(vat);

    /// <summary>
    /// Normalizes a Slovak VAT number.
    /// </summary>
    public static string? Normalize(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return null;
        }

        var sanitized = VatSanitizer.Sanitize(number)!;
        if (sanitized.StartsWith(VatPrefix))
        {
            return sanitized[2..];
        }
        return sanitized;
    }
}