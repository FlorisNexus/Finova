using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Sweden.Validators;

/// <summary>
/// Validator for Swedish VAT numbers (Momsnummer).
/// Format: 12 digits.
/// </summary>
public partial class SwedenVatValidator : VatValidatorBase
{
    [GeneratedRegex(@"^\d{12}$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "SE";

    /// <inheritdoc/>
        public override string CountryCode => VatPrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new SwedenVatValidator().Validate(input);


    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => cleaned.Length == 12;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => VatRegex().IsMatch(cleaned);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned)
    {
        // Checksum Validation (Luhn on first 10 digits)
        string numberPart = cleaned[..10];
        return ChecksumHelper.ValidateLuhn(numberPart)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidSwedenVatChecksum);
    }

    /// <summary>
    /// Static validation method for Swedish VAT numbers.
    /// </summary>
        public static ValidationResult ValidateVat(string? vat) => new SwedenVatValidator().Validate(vat);

    /// <summary>
    /// Gets details for a Swedish VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat) => new SwedenVatValidator().Parse(vat);

    /// <summary>
    /// Normalizes a Swedish VAT number.
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