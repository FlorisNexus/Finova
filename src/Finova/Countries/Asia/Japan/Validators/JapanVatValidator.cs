using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Asia.Japan.Validators;

/// <summary>
/// Validator for Japanese Consumption Tax registration numbers.
/// Japan's consumption tax uses the Corporate Number prefixed with 'T'.
/// Format: T + 13 digits.
/// </summary>
public partial class JapanVatValidator : VatValidatorBase
{
    private const string CountryCodePrefix = "JP";

    /// <inheritdoc/>
        public override string CountryCode => CountryCodePrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new JapanVatValidator().ValidateInternal(input);


    /// <inheritdoc/>
    protected override ValidationResult ValidateInternal(string? vat)
    {
        if (string.IsNullOrWhiteSpace(vat))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var sanitized = VatSanitizer.Sanitize(vat)!;
        var cleaned = sanitized;

        // Remove JP prefix if present
        if (cleaned.StartsWith(CountryCodePrefix, StringComparison.OrdinalIgnoreCase))
        {
            cleaned = cleaned[2..];
        }

        // Check for T prefix (required for qualified invoice issuer)
        bool hasPrefix = cleaned.StartsWith("T", StringComparison.OrdinalIgnoreCase);
        var corporateNumber = hasPrefix ? cleaned[1..] : cleaned;

        // Delegate to Corporate Number validator
        return JapanCorporateNumberValidator.ValidateStatic(corporateNumber);
    }

    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => true; // Handled by JapanCorporateNumberValidator

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => true; // Handled by JapanCorporateNumberValidator

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned) => ValidationResult.Success(); // Handled by JapanCorporateNumberValidator

    /// <inheritdoc/>
    protected override VatDetails CreateDetails(string cleaned)
    {
        // Ensure T prefix for standardized format
        var result = cleaned.StartsWith("T", StringComparison.OrdinalIgnoreCase) ? cleaned : "T" + cleaned;

        return new VatDetails
        {
            VatNumber = result.ToUpperInvariant(),
            CountryCode = CountryCodePrefix,
            IsValid = true,
            IdentifierKind = "Invoice Registration Number",
            IsEuVat = false,
            IsViesEligible = false,
            Notes = "Japanese qualified invoice issuer registration number (適格請求書発行事業者登録番号)"
        };
    }

    /// <summary>
    /// Static validation method for Japanese VAT numbers.
    /// </summary>
    public static ValidationResult ValidateVat(string? vat) => new JapanVatValidator().ValidateInternal(vat);

    /// <summary>
    /// Gets details for a Japanese VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat) => new JapanVatValidator().Parse(vat);
}
