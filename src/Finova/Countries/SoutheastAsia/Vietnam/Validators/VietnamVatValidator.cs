using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.SoutheastAsia.Vietnam.Validators;

/// <summary>
/// Validator for Vietnam VAT (GTGT) number.
/// Reuses the MST (Tax Code) validator.
/// </summary>
public partial class VietnamVatValidator : VatValidatorBase
{
    private const string CountryCodePrefix = "VN";

    /// <inheritdoc/>
        public override string CountryCode => CountryCodePrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new VietnamVatValidator().ValidateInternal(input);


    /// <inheritdoc/>
    protected override ValidationResult ValidateInternal(string? vat)
    {
        if (string.IsNullOrWhiteSpace(vat))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var sanitized = VatSanitizer.Sanitize(vat)!;
        var cleaned = sanitized;

        // Remove VN prefix if present
        if (cleaned.StartsWith(CountryCodePrefix, StringComparison.OrdinalIgnoreCase))
        {
            cleaned = cleaned[2..];
        }

        // Delegate to MST validator
        return new VietnamTaxIdValidator().Validate(cleaned);
    }

    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => true; // Handled by VietnamTaxIdValidator

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => true; // Handled by VietnamTaxIdValidator

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned) => ValidationResult.Success(); // Handled by VietnamTaxIdValidator

    /// <inheritdoc/>
    protected override VatDetails CreateDetails(string cleaned)
    {
        var mst = new VietnamTaxIdValidator().Parse(cleaned);
        return new VatDetails
        {
            VatNumber = mst ?? cleaned,
            CountryCode = CountryCodePrefix,
            IsValid = true,
            IdentifierKind = "MST",
            IsEuVat = false,
            IsViesEligible = false,
            Notes = "Vietnam VAT uses the Tax Code (MST)."
        };
    }

    /// <summary>
    /// Static validation method for Vietnamese VAT numbers.
    /// </summary>
    public static ValidationResult ValidateVat(string? vat) => new VietnamVatValidator().ValidateInternal(vat);

    /// <summary>
    /// Gets details for a Vietnamese VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat) => new VietnamVatValidator().Parse(vat);
}
