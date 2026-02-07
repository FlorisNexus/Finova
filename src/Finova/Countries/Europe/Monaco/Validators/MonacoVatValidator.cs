using Finova.Core.Common;
using Finova.Core.Vat;
using Finova.Countries.Europe.France.Validators;

namespace Finova.Countries.Europe.Monaco.Validators;

/// <summary>
/// Validator for Monaco VAT numbers.
/// Monaco is part of the French VAT system.
/// Format: FR + 2 key digits + 9 SIREN digits.
/// Often represented locally with 'MC' prefix, but technical validation is 'FR'.
/// </summary>
public partial class MonacoVatValidator : VatValidatorBase
{
    private const string CountryCodePrefix = "MC";
    private const string FrenchPrefix = "FR";

    /// <inheritdoc/>
        public override string CountryCode => CountryCodePrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new MonacoVatValidator().ValidateInternal(input);


    /// <inheritdoc/>
    protected override ValidationResult ValidateInternal(string? vat)
    {
        if (string.IsNullOrWhiteSpace(vat))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        // 1. Sanitize (Remove dots, spaces, dashes and convert to uppercase)
        var sanitized = VatSanitizer.Sanitize(vat)!;

        // 2. Handle Monaco specific prefix logic
        string frenchFormatVat;
        if (sanitized.StartsWith("MCFR", StringComparison.OrdinalIgnoreCase))
        {
            frenchFormatVat = sanitized[2..]; // Remove MC, keep FR
        }
        else if (sanitized.StartsWith(CountryCodePrefix, StringComparison.OrdinalIgnoreCase))
        {
            frenchFormatVat = string.Concat(FrenchPrefix, sanitized.AsSpan(2));
        }
        else if (sanitized.StartsWith(FrenchPrefix, StringComparison.OrdinalIgnoreCase))
        {
            frenchFormatVat = sanitized;
        }
        else
        {
            frenchFormatVat = string.Concat(FrenchPrefix, sanitized);
        }

        // 3. Delegate to French validator
        return FranceVatValidator.ValidateVat(frenchFormatVat);
    }

    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => true; // Handled in Validate override

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => true; // Handled in Validate override

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned) => ValidationResult.Success(); // Handled in Validate override

    /// <summary>
    /// Static validation method for Monaco VAT numbers.
    /// </summary>
    public static ValidationResult ValidateVat(string? vat) => new MonacoVatValidator().ValidateInternal(vat);

    /// <summary>
    /// Gets details for a Monaco VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat) => new MonacoVatValidator().Parse(vat);
}
