using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Belgium.Validators;

/// <summary>
/// Validator for Belgian VAT numbers (BTW/TVA).
/// Format: BE0xxx.xxx.xxx or BE0xxxxxxxxx (10 digits total).
/// Note: Belgian VAT numbers are essentially the same as Enterprise Numbers (KBO/BCE) with "BE" prefix.
/// </summary>
public partial class BelgiumVatValidator : VatValidatorBase
{
    [GeneratedRegex(@"[^\d]")]
    private static partial Regex DigitsOnlyRegex();

    private const int VatLength = 10;
    private const string VatPrefix = "BE";

    /// <inheritdoc/>
        public override string CountryCode => VatPrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new BelgiumVatValidator().ValidateInternal(input);


    /// <inheritdoc/>
    protected override ValidationResult ValidateInternal(string? vat)
    {
        if (string.IsNullOrWhiteSpace(vat))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        // Remove "BE" prefix if present
        var sanitized = VatSanitizer.Sanitize(vat)!;
        var cleaned = sanitized;
        if (cleaned.StartsWith(CountryCode))
        {
            cleaned = cleaned[CountryCode.Length..];
        }

        // Auto-correction for historical 9-digit numbers
        if (cleaned.Length == 9)
        {
            cleaned = "0" + cleaned;
        }

        // Delegate to Enterprise Number validator (VAT = KBO/BCE)
        return BelgiumEnterpriseValidator.ValidateEnterpriseNumber(cleaned);
    }

    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => cleaned.Length == 10 || cleaned.Length == 9;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => true; // Handled by EnterpriseValidator

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned)
    {
        var toValidate = cleaned;
        if (toValidate.Length == 9)
        {
            toValidate = "0" + toValidate;
        }
        return BelgiumEnterpriseValidator.ValidateEnterpriseNumber(toValidate);
    }

    /// <summary>
    /// Formats a Belgian VAT number in the standard format: BE 0xxx.xxx.xxx.
    /// </summary>
    /// <param name="vat">The VAT number to format</param>
    /// <returns>Formatted VAT number</returns>
    /// <exception cref="ArgumentException">If the VAT number is invalid</exception>
    public static string Format(string? vat)
    {
        var validator = new BelgiumVatValidator();
        if (!validator.Validate(vat).IsValid)
        {
            throw new ArgumentException("Invalid Belgian VAT number", nameof(vat));
        }

        // Remove BE prefix if present
        var cleaned = vat!.Trim().ToUpperInvariant();
        if (cleaned.StartsWith(VatPrefix))
        {
            cleaned = cleaned[VatPrefix.Length..];
        }

        var digits = DigitsOnlyRegex().Replace(cleaned, "");

        // Format as: BE 0xxx.xxx.xxx
        return $"{VatPrefix} {digits[0]}{digits.Substring(1, 3)}.{digits.Substring(4, 3)}.{digits.Substring(7, 3)}";
    }

    /// <summary>
    /// Normalizes a Belgian VAT number by keeping only digits and the BE prefix.
    /// </summary>
    /// <param name="vat">The VAT number to normalize</param>
    /// <returns>Normalized VAT number (BExxxxxxxxxx)</returns>
    public static string Normalize(string? vat)
    {
        if (string.IsNullOrWhiteSpace(vat))
        {
            return string.Empty;
        }

        var cleaned = vat.Trim().ToUpperInvariant();
        var digits = DigitsOnlyRegex().Replace(cleaned, "");

        if (digits.Length == VatLength)
        {
            return $"{VatPrefix}{digits}";
        }

        return string.Empty;
    }

    /// <summary>
    /// Extracts the Enterprise Number (KBO/BCE) from a VAT number.
    /// </summary>
    /// <param name="vat">The VAT number</param>
    /// <returns>The corresponding KBO/BCE number or null if invalid</returns>
    public static string? GetEnterpriseNumber(string? vat)
    {
        var validator = new BelgiumVatValidator();
        if (!validator.Validate(vat).IsValid)
        {
            return null;
        }

        var cleaned = vat!.Trim().ToUpperInvariant();
        if (cleaned.StartsWith(VatPrefix))
        {
            cleaned = cleaned[VatPrefix.Length..];
        }

        return BelgiumEnterpriseValidator.Normalize(cleaned);
    }

    /// <summary>
    /// Parses a Belgian VAT number and returns details.
    /// </summary>
    /// <param name="vat">The VAT number to parse.</param>
    /// <returns>VatDetails object or null if invalid.</returns>
    public static Core.Vat.VatDetails? GetVatDetails(string? vat)
    {
        var validator = new BelgiumVatValidator();
        return validator.Parse(vat);
    }

    /// <summary>
    /// Static validation method for Belgian VAT numbers.
    /// </summary>
    public static ValidationResult ValidateVat(string? vat) => new BelgiumVatValidator().Validate(vat);
}
