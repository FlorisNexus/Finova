using Finova.Core.Common;
using Finova.Core.Vat;
using Finova.Countries.Europe.Switzerland.Validators;

namespace Finova.Countries.Europe.Liechtenstein.Validators;

/// <summary>
/// Validator for Liechtenstein VAT numbers.
/// Liechtenstein uses Swiss VAT numbers (UID/MWST).
/// Format: CHE-123.456.789 (or without dashes/dots).
/// </summary>
public partial class LiechtensteinVatValidator : VatValidatorBase
{
    private const string SwissPrefix = "CHE";
    private const string CountryCodePrefix = "LI";

    /// <inheritdoc/>
        public override string CountryCode => CountryCodePrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new LiechtensteinVatValidator().ValidateInternal(input);


    /// <inheritdoc/>
    protected override ValidationResult ValidateInternal(string? vat)
    {
        if (string.IsNullOrWhiteSpace(vat))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var sanitized = VatSanitizer.Sanitize(vat)!;
        var cleaned = sanitized;

        // Remove LI prefix if present
        if (cleaned.StartsWith(CountryCodePrefix, StringComparison.OrdinalIgnoreCase))
        {
            cleaned = cleaned[2..];
        }

        // Liechtenstein uses Swiss VAT system
        return SwitzerlandVatValidator.ValidateVat(cleaned);
    }

    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => true; // Handled by SwitzerlandVatValidator

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => true; // Handled by SwitzerlandVatValidator

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned) => ValidationResult.Success(); // Handled by SwitzerlandVatValidator

    /// <inheritdoc/>
    protected override VatDetails CreateDetails(string cleaned)
    {
        // Extract normalized Swiss number (remove CHE if present in the cleaned part)
        var result = cleaned;
        if (result.StartsWith(SwissPrefix, StringComparison.OrdinalIgnoreCase))
        {
            result = result[SwissPrefix.Length..];
        }

        return new VatDetails
        {
            CountryCode = CountryCodePrefix,
            VatNumber = result,
            IsValid = true
        };
    }

    /// <summary>
    /// Static validation method for Liechtenstein VAT numbers.
    /// </summary>
    public static ValidationResult ValidateVat(string? vat) => new LiechtensteinVatValidator().ValidateInternal(vat);

    /// <summary>
    /// Gets details for a Liechtenstein VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat) => new LiechtensteinVatValidator().Parse(vat);
}