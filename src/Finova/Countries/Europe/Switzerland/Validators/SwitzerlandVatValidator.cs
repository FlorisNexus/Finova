using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Switzerland.Validators;

/// <summary>
/// Validator for Swiss VAT numbers (UID/MWST).
/// Format: CHE-123.456.789 or 9 digits.
/// </summary>
public partial class SwitzerlandVatValidator : VatValidatorBase
{
    [GeneratedRegex(@"^\d{9}$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "CHE";
    private const string AltPrefix = "CH";

    private static readonly int[] Weights = { 5, 4, 3, 2, 7, 6, 5, 4 };

    /// <inheritdoc/>
        public override string CountryCode => VatPrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new SwitzerlandVatValidator().ValidateInternal(input);


    /// <inheritdoc/>
    protected override ValidationResult ValidateInternal(string? vat)
    {
        if (string.IsNullOrWhiteSpace(vat))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var sanitized = VatSanitizer.Sanitize(vat)!;
        var cleaned = sanitized;

        // Handle Swiss prefixes: CHE, CH
        if (cleaned.StartsWith(VatPrefix, StringComparison.OrdinalIgnoreCase))
        {
            cleaned = cleaned[3..];
        }
        else if (cleaned.StartsWith(AltPrefix, StringComparison.OrdinalIgnoreCase))
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
        int sum = ChecksumHelper.CalculateWeightedSum(cleaned.Substring(0, 8), Weights);
        int remainder = sum % 11;
        int checkDigit = 11 - remainder;

        if (checkDigit == 11)
        {
            checkDigit = 0;
        }

        if (checkDigit == 10)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, string.Format(ValidationMessages.InvalidVatChecksum, "Switzerland"));
        }

        int lastDigit = cleaned[8] - '0';
        return checkDigit == lastDigit
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, string.Format(ValidationMessages.InvalidVatChecksum, "Switzerland"));
    }

    /// <inheritdoc/>
    protected override VatDetails CreateDetails(string cleaned)
    {
        // Extract normalized Swiss number (remove CHE if present in the cleaned part)
        var result = cleaned;
        if (result.StartsWith(VatPrefix, StringComparison.OrdinalIgnoreCase))
        {
            result = result[VatPrefix.Length..];
        }

        return new VatDetails
        {
            CountryCode = CountryCode,
            VatNumber = result,
            IsValid = true
        };
    }

    /// <summary>
    /// Static validation method for Swiss VAT numbers.
    /// </summary>
    public static ValidationResult ValidateVat(string? vat) => new SwitzerlandVatValidator().ValidateInternal(vat);

    /// <summary>
    /// Gets details for a Swiss VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat) => new SwitzerlandVatValidator().Parse(vat);
}