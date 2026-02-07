using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Austria.Validators;

/// <summary>
/// Validator for Austrian VAT numbers (UID-Nummer).
/// Format: 9 characters. 'U' + 8 digits.
/// </summary>
public partial class AustriaVatValidator : VatValidatorBase
{
    private const string CountryCodePrefix = "AT";

    [GeneratedRegex(@"^U\d{8}$")]
    private static partial Regex AustriaVatRegex();

    /// <inheritdoc/>
        public override string CountryCode => CountryCodePrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new AustriaVatValidator().ValidateInternal(input);


    /// <inheritdoc/>
    protected override ValidationResult ValidateInternal(string? vat)
    {
        if (string.IsNullOrWhiteSpace(vat))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var sanitized = VatSanitizer.Sanitize(vat)!;
        var cleaned = sanitized;

        // Special handling for Austria prefixes: AT, ATU, U
        if (cleaned.StartsWith("ATU", StringComparison.OrdinalIgnoreCase))
        {
            cleaned = cleaned[2..]; // Keep the 'U'
        }
        else if (cleaned.StartsWith("AT", StringComparison.OrdinalIgnoreCase))
        {
            cleaned = "U" + cleaned[2..];
        }
        else if (cleaned.Length == 8 && long.TryParse(cleaned, out _))
        {
            cleaned = "U" + cleaned;
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
    protected override bool ValidateFormat(string cleaned) => AustriaVatRegex().IsMatch(cleaned);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned)
    {
        // For ATU12345678, we validate '1234567' with check digit '8'
        string digitsStr = cleaned.Substring(1, 7);
        int checkDigit = cleaned[8] - '0';

        int[] weights = { 1, 2, 1, 2, 1, 2, 1 };
        int sum = ChecksumHelper.CalculateLuhnStyleWeightedSum(digitsStr, weights);

        int total = (sum + 4);
        int remainder = total % 10;
        int calculated = (10 - remainder) % 10;

        return calculated == checkDigit
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }

    /// <inheritdoc/>
    protected override VatDetails CreateDetails(string cleaned)
    {
        return new VatDetails
        {
            CountryCode = CountryCode,
            VatNumber = cleaned.StartsWith("U") ? cleaned : "U" + cleaned,
            IsValid = true
        };
    }

    /// <summary>
    /// Static validation method for Austrian VAT numbers.
    /// </summary>
    public static ValidationResult ValidateVat(string? vat) => new AustriaVatValidator().ValidateInternal(vat);

    /// <summary>
    /// Gets details for an Austrian VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat) => new AustriaVatValidator().Parse(vat);
}
