using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Africa.Egypt.Validators;

/// <summary>
/// Validator for Egyptian VAT numbers (Tax Registration Number - TRN).
/// Reuses the TRN validator.
/// </summary>
public partial class EgyptVatValidator : VatValidatorBase
{
    private const string CountryCodePrefix = "EG";

    /// <inheritdoc/>
        public override string CountryCode => CountryCodePrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new EgyptVatValidator().ValidateInternal(input);


    /// <inheritdoc/>
    protected override ValidationResult ValidateInternal(string? vat)
    {
        if (string.IsNullOrWhiteSpace(vat))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var sanitized = VatSanitizer.Sanitize(vat)!;
        var cleaned = sanitized;

        // Remove EG prefix if present
        if (cleaned.StartsWith(CountryCodePrefix, StringComparison.OrdinalIgnoreCase))
        {
            cleaned = cleaned[2..];
        }

        // Delegate to TRN validator
        return new EgyptTaxRegistrationNumberValidator().Validate(cleaned);
    }

    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => true; // Handled by EgyptTaxRegistrationNumberValidator

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => true; // Handled by EgyptTaxRegistrationNumberValidator

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned) => ValidationResult.Success(); // Handled by EgyptTaxRegistrationNumberValidator

    /// <inheritdoc/>
    protected override VatDetails CreateDetails(string cleaned)
    {
        var trn = new EgyptTaxRegistrationNumberValidator().Parse(cleaned);
        return new VatDetails
        {
            VatNumber = trn ?? cleaned,
            CountryCode = CountryCodePrefix,
            IsValid = true,
            IdentifierKind = "TRN",
            IsEuVat = false,
            IsViesEligible = false,
            Notes = "Egypt VAT uses the Tax Registration Number (TRN)."
        };
    }

    /// <summary>
    /// Static validation method for Egyptian VAT numbers.
    /// </summary>
    public static ValidationResult ValidateVat(string? vat) => new EgyptVatValidator().ValidateInternal(vat);

    /// <summary>
    /// Gets details for an Egyptian VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat) => new EgyptVatValidator().Parse(vat);
}
