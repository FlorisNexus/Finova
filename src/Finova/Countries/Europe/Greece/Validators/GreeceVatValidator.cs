using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Greece.Validators;

/// <summary>
/// Validator for Greek VAT numbers (AFM).
/// Format: 9 digits.
/// </summary>
public partial class GreeceVatValidator : VatValidatorBase
{
    [GeneratedRegex(@"^\d{9}$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "EL"; // Greece uses EL for VAT

    /// <inheritdoc/>
        public override string CountryCode => VatPrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new GreeceVatValidator().ValidateInternal(input);


    /// <inheritdoc/>
    protected override ValidationResult ValidateInternal(string? vat)
    {
        if (string.IsNullOrWhiteSpace(vat))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var sanitized = VatSanitizer.Sanitize(vat)!;
        var cleaned = sanitized;

        // Special handling for Greece prefixes: EL, GR
        if (cleaned.StartsWith("EL", StringComparison.OrdinalIgnoreCase) || 
            cleaned.StartsWith("GR", StringComparison.OrdinalIgnoreCase))
        {
            cleaned = cleaned[2..];
        }

        if (!IsValidLength(cleaned))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength,
                string.Format(ValidationMessages.InvalidLength, CountryCode));
        }

        if (!ValidateFormat(cleaned))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat,
                string.Format(ValidationMessages.InvalidVatFormat, CountryCode));
        }

        return ValidateChecksum(cleaned);
    }

    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => cleaned.Length == 9;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => VatRegex().IsMatch(cleaned);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned)
    {
        // Weights: 256, 128, 64, 32, 16, 8, 4, 2
        int[] weights = { 256, 128, 64, 32, 16, 8, 4, 2 };

        int sum = ChecksumHelper.CalculateWeightedSum(cleaned.Substring(0, 8), weights);

        int remainder = sum % 11;
        int checkDigit = remainder % 10; // If remainder is 10, check digit is 0.

        int lastDigit = cleaned[8] - '0';
        return checkDigit == lastDigit
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidGreeceVatChecksum);
    }

    /// <summary>
    /// Static validation method for Greek VAT numbers.
    /// </summary>
    public static ValidationResult ValidateVat(string? vat) => new GreeceVatValidator().ValidateInternal(vat);

    /// <summary>
    /// Gets details for a Greek VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat) => new GreeceVatValidator().Parse(vat);
}
