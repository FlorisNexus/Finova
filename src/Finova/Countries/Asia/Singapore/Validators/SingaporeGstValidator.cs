using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Asia.Singapore.Validators;

/// <summary>
/// Validator for Singapore Goods and Services Tax (GST) registration number.
/// GST registration uses UEN (Unique Entity Number).
/// </summary>
public partial class SingaporeGstValidator : VatValidatorBase
{
    private const string CountryCodePrefix = "SG";

    /// <inheritdoc/>
        public override string CountryCode => CountryCodePrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new SingaporeGstValidator().ValidateInternal(input);


    /// <inheritdoc/>
    protected override ValidationResult ValidateInternal(string? vat)
    {
        if (string.IsNullOrWhiteSpace(vat))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var sanitized = VatSanitizer.Sanitize(vat)!;
        var cleaned = sanitized;

        // Remove common GST suffixes if present
        if (cleaned.EndsWith("GST", StringComparison.OrdinalIgnoreCase))
        {
            cleaned = cleaned[..^3];
        }

        // Remove SG prefix if present
        if (cleaned.StartsWith(CountryCodePrefix, StringComparison.OrdinalIgnoreCase))
        {
            cleaned = cleaned[2..];
        }

        // Delegate to UEN validator
        return SingaporeUenValidator.ValidateStatic(cleaned);
    }

    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => true; // Handled by SingaporeUenValidator

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => true; // Handled by SingaporeUenValidator

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned) => ValidationResult.Success(); // Handled by SingaporeUenValidator

    /// <inheritdoc/>
    protected override VatDetails CreateDetails(string cleaned)
    {
        return new VatDetails
        {
            VatNumber = cleaned,
            CountryCode = CountryCodePrefix,
            IsValid = true,
            IdentifierKind = "GST",
            IsEuVat = false,
            IsViesEligible = false,
            Notes = "Singapore Goods and Services Tax (based on UEN)"
        };
    }

    /// <summary>
    /// Static validation method for Singaporean GST numbers.
    /// </summary>
    public static ValidationResult ValidateVat(string? vat) => new SingaporeGstValidator().ValidateInternal(vat);

    /// <summary>
    /// Gets details for a Singaporean GST number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat) => new SingaporeGstValidator().Parse(vat);
}
