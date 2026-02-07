using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.UnitedKingdom.Validators;

/// <summary>
/// Validator for United Kingdom VAT numbers.
/// Format: GB followed by 9 or 12 digits, or special prefixes GD or HA.
/// </summary>
public partial class UnitedKingdomVatValidator : VatValidatorBase
{
    [GeneratedRegex(@"^(\d{9}|\d{12}|GD\d{3}|HA\d{3})$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "GB";

    /// <inheritdoc/>
        public override string CountryCode => VatPrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new UnitedKingdomVatValidator().Validate(input);


    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned)
    {
        if (cleaned.StartsWith("GD") || cleaned.StartsWith("HA"))
        {
            return cleaned.Length == 5;
        }
        return cleaned.Length == 9 || cleaned.Length == 12;
    }

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => VatRegex().IsMatch(cleaned);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned)
    {
        if (cleaned.StartsWith("GD") || cleaned.StartsWith("HA"))
        {
            return ValidationResult.Success(); // No checksum for these
        }

        if (!long.TryParse(cleaned, out _))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.MustContainOnlyDigits);
        }

        // Checksum Validation (Weighted Mod 97)
        // Only for first 9 digits (even if 12 digits, first 9 are the block)
        string block = cleaned.Substring(0, 9);

        // Weights: 8, 7, 6, 5, 4, 3, 2
        int[] weights = { 8, 7, 6, 5, 4, 3, 2 };

        int sum = ChecksumHelper.CalculateWeightedSum(block.Substring(0, 7), weights);

        int checkDigits = int.Parse(block.Substring(7, 2));
        long totalSum = sum + checkDigits;

        if (totalSum % 97 == 0)
        {
            return ValidationResult.Success();
        }

        // UK has a secondary algorithm (add 55 to sum)
        if ((totalSum + 55) % 97 == 0)
        {
            return ValidationResult.Success();
        }

        return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, string.Format(ValidationMessages.InvalidVatChecksum, "UK"));
    }

    /// <summary>
    /// Static validation method for United Kingdom VAT numbers.
    /// </summary>
        public static ValidationResult ValidateVat(string? vat) => new UnitedKingdomVatValidator().Validate(vat);

    /// <summary>
    /// Gets details for a United Kingdom VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat) => new UnitedKingdomVatValidator().Parse(vat);
}