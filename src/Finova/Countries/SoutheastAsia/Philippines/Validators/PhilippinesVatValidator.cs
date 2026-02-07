using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.SoutheastAsia.Philippines.Validators;

/// <summary>
/// Validator for Philippines VAT number (TIN).
/// Reuses the TIN validator.
/// </summary>
public partial class PhilippinesVatValidator : VatValidatorBase
{
    private const string CountryCodePrefix = "PH";

    /// <inheritdoc/>
        public override string CountryCode => CountryCodePrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new PhilippinesVatValidator().ValidateInternal(input);


    /// <inheritdoc/>
    protected override ValidationResult ValidateInternal(string? vat)
    {
        if (string.IsNullOrWhiteSpace(vat))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var sanitized = VatSanitizer.Sanitize(vat)!;
        var cleaned = sanitized;

        // Remove PH prefix if present
        if (cleaned.StartsWith(CountryCodePrefix, StringComparison.OrdinalIgnoreCase))
        {
            cleaned = cleaned[2..];
        }

        // Delegate to TIN validator
        return PhilippinesTinValidator.ValidateStatic(cleaned);
    }

    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => true; // Handled by PhilippinesTinValidator

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => true; // Handled by PhilippinesTinValidator

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned) => ValidationResult.Success(); // Handled by PhilippinesTinValidator

    /// <inheritdoc/>
    protected override VatDetails CreateDetails(string cleaned)
    {
        var tinDetails = new PhilippinesTinValidator().Parse(cleaned);
        return new VatDetails
        {
            VatNumber = tinDetails?.VatNumber ?? cleaned,
            CountryCode = CountryCodePrefix,
            IsValid = true,
            IdentifierKind = "TIN",
            IsEuVat = false,
            IsViesEligible = false,
            Notes = "Philippines VAT uses the TIN."
        };
    }

    /// <summary>
    /// Static validation method for Philippines VAT numbers.
    /// </summary>
    public static ValidationResult ValidateVat(string? vat) => new PhilippinesVatValidator().ValidateInternal(vat);

    /// <summary>
    /// Gets details for a Philippines VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat) => new PhilippinesVatValidator().Parse(vat);
}