using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Russia.Validators;

/// <summary>
/// Validator for Russian VAT numbers (NDS).
/// Russian VAT numbers are identical to INN (Individual Taxpayer Number).
/// Format: 10 or 12 digits.
/// </summary>
public partial class RussiaVatValidator : VatValidatorBase
{
    private const string CountryCodePrefix = "RU";

    /// <inheritdoc/>
        public override string CountryCode => CountryCodePrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new RussiaVatValidator().ValidateInternal(input);


    /// <inheritdoc/>
    protected override ValidationResult ValidateInternal(string? vat)
    {
        if (string.IsNullOrWhiteSpace(vat))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var sanitized = VatSanitizer.Sanitize(vat)!;
        var cleaned = sanitized;

        // Remove RU prefix if present
        if (cleaned.StartsWith(CountryCodePrefix, StringComparison.OrdinalIgnoreCase))
        {
            cleaned = cleaned[2..];
        }

        // Delegate to INN validator
        return new RussiaInnValidator().Validate(cleaned);
    }

    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => true; // Handled by RussiaInnValidator

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => true; // Handled by RussiaInnValidator

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned) => ValidationResult.Success(); // Handled by RussiaInnValidator

    /// <inheritdoc/>
    protected override VatDetails CreateDetails(string cleaned)
    {
        var innValidator = new RussiaInnValidator() as IValidator<string>;
        var inn = innValidator.Parse(cleaned);
        
        return new VatDetails
        {
            VatNumber = inn ?? cleaned,
            CountryCode = CountryCodePrefix,
            IsValid = true,
            IdentifierKind = "INN",
            IsEuVat = false,
            IsViesEligible = false,
            Notes = "Russia VAT (NDS) uses the INN number."
        };
    }

    /// <summary>
    /// Static validation method for Russian VAT numbers.
    /// </summary>
    public static ValidationResult ValidateVat(string? vat) => new RussiaVatValidator().ValidateInternal(vat);

    /// <summary>
    /// Gets details for a Russian VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat) => new RussiaVatValidator().Parse(vat);
}
