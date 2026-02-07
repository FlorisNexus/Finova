using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.SoutheastAsia.Indonesia.Validators;

/// <summary>
/// Validator for Indonesian VAT identifier (PPN).
/// Indonesian VAT numbers are identical to NPWP (Nomor Pokok Wajib Pajak).
/// Format: 15 digits.
/// </summary>
public partial class IndonesiaVatValidator : VatValidatorBase
{
    private const string CountryCodePrefix = "ID";

    /// <inheritdoc/>
        public override string CountryCode => CountryCodePrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new IndonesiaVatValidator().ValidateInternal(input);


    /// <inheritdoc/>
    protected override ValidationResult ValidateInternal(string? vat)
    {
        if (string.IsNullOrWhiteSpace(vat))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var sanitized = VatSanitizer.Sanitize(vat)!;
        var cleaned = sanitized;

        // Remove ID prefix if present
        if (cleaned.StartsWith(CountryCodePrefix, StringComparison.OrdinalIgnoreCase))
        {
            cleaned = cleaned[2..];
        }

        // Delegate to NPWP validator
        return IndonesiaNpwpValidator.ValidateStatic(cleaned);
    }

    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => true; // Handled by IndonesiaNpwpValidator

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => true; // Handled by IndonesiaNpwpValidator

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned) => ValidationResult.Success(); // Handled by IndonesiaNpwpValidator

    /// <inheritdoc/>
    protected override VatDetails CreateDetails(string cleaned)
    {
        var npwp = new IndonesiaNpwpValidator().Parse(cleaned);
        return new VatDetails
        {
            VatNumber = npwp ?? cleaned,
            CountryCode = CountryCodePrefix,
            IsValid = true,
            IdentifierKind = "NPWP",
            IsEuVat = false,
            IsViesEligible = false,
            Notes = "Indonesia VAT (PPN) uses the NPWP number."
        };
    }

    /// <summary>
    /// Static validation method for Indonesian VAT numbers.
    /// </summary>
    public static ValidationResult ValidateVat(string? vat) => new IndonesiaVatValidator().ValidateInternal(vat);

    /// <summary>
    /// Gets details for an Indonesian VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat) => new IndonesiaVatValidator().Parse(vat);
}
