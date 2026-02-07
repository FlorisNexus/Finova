using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Slovenia.Validators;

/// <summary>
/// Validator for Slovenian VAT numbers (ID za DDV).
/// Format: 8 digits.
/// </summary>
public partial class SloveniaVatValidator : VatValidatorBase
{
    [GeneratedRegex(@"^\d{8}$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "SI";
    private static readonly int[] Weights = [8, 7, 6, 5, 4, 3, 2];

    /// <inheritdoc/>
        public override string CountryCode => VatPrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new SloveniaVatValidator().Validate(input);


    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => cleaned.Length == 8;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => VatRegex().IsMatch(cleaned);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned)
    {
        int sum = ChecksumHelper.CalculateWeightedSum(cleaned[..7], Weights);
        int remainder = sum % 11;

        if (remainder == 1)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidSloveniaVatChecksumForbidden);
        }

        int checkDigit = remainder == 0 ? 0 : 11 - remainder;

        return checkDigit == (cleaned[7] - '0')
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidSloveniaVatChecksum);
    }

    /// <summary>
    /// Static validation method for Slovenian VAT numbers.
    /// </summary>
        public static ValidationResult ValidateVat(string? vat) => new SloveniaVatValidator().Validate(vat);

    /// <summary>
    /// Gets details for a Slovenian VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat) => new SloveniaVatValidator().Parse(vat);

    /// <summary>
    /// Normalizes a Slovenian VAT number.
    /// Returns null if the input is not a valid Slovenian VAT number.
    /// </summary>
    public static string? Normalize(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return null;
        }

        var result = ValidateVat(number);
        if (!result.IsValid)
        {
            return null;
        }

        var sanitized = VatSanitizer.Sanitize(number)!;
        if (sanitized.StartsWith(VatPrefix))
        {
            return sanitized[2..];
        }
        return sanitized;
    }
}
