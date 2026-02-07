using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Turkey.Validators;

/// <summary>
/// Validator for Turkish VAT numbers (KDV).
/// Turkish VAT numbers are identical to VKN (Vergi Kimlik Numarası).
/// Format: 10 digits.
/// </summary>
public partial class TurkeyVatValidator : VatValidatorBase
{
    private const string CountryCodePrefix = "TR";

    /// <inheritdoc/>
        public override string CountryCode => CountryCodePrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new TurkeyVatValidator().ValidateInternal(input);


    /// <inheritdoc/>
    protected override ValidationResult ValidateInternal(string? vat)
    {
        if (string.IsNullOrWhiteSpace(vat))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var sanitized = VatSanitizer.Sanitize(vat)!;
        var cleaned = sanitized;

        // Remove TR prefix if present
        if (cleaned.StartsWith(CountryCodePrefix, StringComparison.OrdinalIgnoreCase))
        {
            cleaned = cleaned[2..];
        }

        // Delegate to VKN validator
        return new TurkeyVknValidator().Validate(cleaned);
    }

    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => true; // Handled by TurkeyVknValidator

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => true; // Handled by TurkeyVknValidator

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned) => ValidationResult.Success(); // Handled by TurkeyVknValidator

    /// <inheritdoc/>
    protected override VatDetails CreateDetails(string cleaned)
    {
        var vknValidator = new TurkeyVknValidator() as IValidator<string>;
        var vknNormalized = vknValidator.Parse(cleaned);
        
        return new VatDetails
        {
            VatNumber = vknNormalized ?? cleaned,
            CountryCode = CountryCodePrefix,
            IsValid = true,
            IdentifierKind = "VKN",
            IsEuVat = false,
            IsViesEligible = false,
            Notes = "Turkey VAT (KDV) uses the VKN number."
        };
    }

    /// <summary>
    /// Static validation method for Turkish VAT numbers.
    /// </summary>
    public static ValidationResult ValidateVat(string? vat) => new TurkeyVatValidator().ValidateInternal(vat);

    /// <summary>
    /// Gets details for a Turkish VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat) => new TurkeyVatValidator().Parse(vat);
}
