using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Norway.Validators;

/// <summary>
/// Validator for Norwegian VAT numbers (MVA).
/// Format: 9 digits + optional MVA suffix.
/// </summary>
public partial class NorwayVatValidator : VatValidatorBase
{
    [GeneratedRegex(@"^\d{9}$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "NO";

    /// <inheritdoc/>
        public override string CountryCode => VatPrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new NorwayVatValidator().ValidateInternal(input);


    /// <inheritdoc/>
    protected override ValidationResult ValidateInternal(string? vat)
    {
        if (string.IsNullOrWhiteSpace(vat))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var sanitized = VatSanitizer.Sanitize(vat)!;
        var cleaned = sanitized;

        // Remove NO prefix if present
        if (cleaned.StartsWith(VatPrefix, StringComparison.OrdinalIgnoreCase))
        {
            cleaned = cleaned[2..];
        }

        // Remove MVA suffix if present
        if (cleaned.EndsWith("MVA", StringComparison.OrdinalIgnoreCase))
        {
            cleaned = cleaned[..^3];
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
        // Weights: 3, 2, 7, 6, 5, 4, 3, 2
        int[] weights = { 3, 2, 7, 6, 5, 4, 3, 2 };

        int sum = ChecksumHelper.CalculateWeightedSum(cleaned.Substring(0, 8), weights);

        int remainder = sum % 11;
        int checkDigit = 11 - remainder;
        if (checkDigit == 11)
        {
            checkDigit = 0;
        }

        if (checkDigit == 10)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidNorwayVatChecksumForbidden);
        }

        int lastDigit = cleaned[8] - '0';
        return checkDigit == lastDigit
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidNorwayVatChecksum);
    }

    /// <inheritdoc/>
    protected override VatDetails CreateDetails(string cleaned)
    {
        return new VatDetails
        {
            CountryCode = CountryCode,
            VatNumber = cleaned,
            IsValid = true
        };
    }

    /// <summary>
    /// Static validation method for Norwegian VAT numbers.
    /// </summary>
    public static ValidationResult ValidateVat(string? vat) => new NorwayVatValidator().ValidateInternal(vat);

    /// <summary>
    /// Gets details for a Norwegian VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat) => new NorwayVatValidator().Parse(vat);
}
