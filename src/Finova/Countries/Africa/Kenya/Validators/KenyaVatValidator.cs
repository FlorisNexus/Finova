using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Africa.Kenya.Validators;

/// <summary>
/// Validator for Kenyan VAT numbers (KRA PIN).
/// Reuses the KRA PIN validator.
/// </summary>
public partial class KenyaVatValidator : VatValidatorBase
{
    private const string CountryCodePrefix = "KE";

    /// <inheritdoc/>
        public override string CountryCode => CountryCodePrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new KenyaVatValidator().ValidateInternal(input);


    /// <inheritdoc/>
    protected override ValidationResult ValidateInternal(string? vat)
    {
        if (string.IsNullOrWhiteSpace(vat))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var sanitized = VatSanitizer.Sanitize(vat)!;
        var cleaned = sanitized;

        // Remove KE prefix if present
        if (cleaned.StartsWith(CountryCodePrefix, StringComparison.OrdinalIgnoreCase))
        {
            cleaned = cleaned[2..];
        }

        // Delegate to PIN validator
        return KenyaPinValidator.ValidateStatic(cleaned);
    }

    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => true; // Handled by KenyaPinValidator

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => true; // Handled by KenyaPinValidator

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned) => ValidationResult.Success(); // Handled by KenyaPinValidator

    /// <inheritdoc/>
    protected override VatDetails CreateDetails(string cleaned)
    {
        var pin = new KenyaPinValidator().Parse(cleaned);
        return new VatDetails
        {
            VatNumber = pin ?? cleaned,
            CountryCode = CountryCodePrefix,
            IsValid = true,
            IdentifierKind = "PIN",
            IsEuVat = false,
            IsViesEligible = false,
            Notes = "Kenya VAT uses the KRA PIN."
        };
    }

    /// <summary>
    /// Static validation method for Kenyan VAT numbers.
    /// </summary>
    public static ValidationResult ValidateVat(string? vat) => new KenyaVatValidator().ValidateInternal(vat);

    /// <summary>
    /// Gets details for a Kenyan VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat) => new KenyaVatValidator().Parse(vat);
}
