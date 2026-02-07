using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Luxembourg.Validators;

/// <summary>
/// Validator for Luxembourg VAT numbers (TVA).
/// Format: 8 digits.
/// </summary>
public partial class LuxembourgVatValidator : VatValidatorBase
{
    private const string VatPrefix = "LU";

    /// <inheritdoc/>
        public override string CountryCode => VatPrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new LuxembourgVatValidator().Validate(input);


    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => cleaned.Length == 8;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => long.TryParse(cleaned, out _);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned)
    {
        // Checksum Validation (Mod 89)
        // First 6 digits % 89 == Last 2 digits
        int firstPart = int.Parse(cleaned[..6]);
        int checkDigits = int.Parse(cleaned[^2..]);

        if (firstPart % 89 != checkDigits)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidLuxembourgVatChecksum);
        }

        return ValidationResult.Success();
    }

    /// <summary>
    /// Static validation method for Luxembourg VAT numbers.
    /// </summary>
        public static ValidationResult ValidateVat(string? vat) => new LuxembourgVatValidator().Validate(vat);

    /// <summary>
    /// Gets details for a Luxembourg VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat) => new LuxembourgVatValidator().Parse(vat);

    /// <summary>
    /// Normalizes a Luxembourg VAT number.
    /// </summary>
    public static string Normalize(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return string.Empty;
        }

        var sanitized = VatSanitizer.Sanitize(number)!;
        if (sanitized.StartsWith(VatPrefix))
        {
            return sanitized[2..];
        }
        return sanitized;
    }
}