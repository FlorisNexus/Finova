using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Malta.Validators;

/// <summary>
/// Validator for Maltese VAT numbers.
/// Format: 8 digits.
/// </summary>
public partial class MaltaVatValidator : VatValidatorBase
{
    [GeneratedRegex(@"^\d{8}$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "MT";

    /// <inheritdoc/>
        public override string CountryCode => VatPrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new MaltaVatValidator().Validate(input);


    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => cleaned.Length == 8;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => VatRegex().IsMatch(cleaned);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned)
    {
        // Weights: 3, 4, 6, 7, 8, 9, 10, 1 applied to the 8 digits.
        int[] weights = { 3, 4, 6, 7, 8, 9, 10, 1 };

        int sum = ChecksumHelper.CalculateWeightedSum(cleaned, weights);

        return sum % 37 == 0
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidMaltaVatChecksum);
    }

    /// <summary>
    /// Static validation method for Maltese VAT numbers.
    /// </summary>
        public static ValidationResult ValidateVat(string? vat) => new MaltaVatValidator().Validate(vat);

    /// <summary>
    /// Gets details for a Maltese VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat) => new MaltaVatValidator().Parse(vat);

    /// <summary>
    /// Normalizes a Maltese VAT number.
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