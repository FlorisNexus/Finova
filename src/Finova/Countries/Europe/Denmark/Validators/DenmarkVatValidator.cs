using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Denmark.Validators;

/// <summary>
/// Validator for Danish VAT numbers (CVR).
/// Format: 8 digits.
/// </summary>
public partial class DenmarkVatValidator : VatValidatorBase
{
    [GeneratedRegex(@"^\d{8}$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "DK";

    /// <inheritdoc/>
        public override string CountryCode => VatPrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new DenmarkVatValidator().Validate(input);


    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => cleaned.Length == 8;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => VatRegex().IsMatch(cleaned);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned)
    {
        int[] weights = { 2, 7, 6, 5, 4, 3, 2, 1 };

        return ChecksumHelper.ValidateWeightedModulo11(cleaned, weights, r => r == 0)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidDenmarkVatChecksum);
    }

    /// <summary>
    /// Static validation method for Danish VAT numbers.
    /// </summary>
        public static ValidationResult ValidateVat(string? vat) => new DenmarkVatValidator().Validate(vat);

    /// <summary>
    /// Gets details for a Danish VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat) => new DenmarkVatValidator().Parse(vat);
}