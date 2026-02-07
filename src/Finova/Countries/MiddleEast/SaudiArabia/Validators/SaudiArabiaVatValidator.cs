using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.MiddleEast.SaudiArabia.Validators;

/// <summary>
/// Validator for Saudi Arabian VAT Registration Number.
/// Format: 15 digits starting with 3.
/// </summary>
public partial class SaudiArabiaVatValidator : VatValidatorBase
{
    private const string CountryCodePrefix = "SA";

    [GeneratedRegex(@"^3\d{14}$")]
    private static partial Regex VatRegex();

    /// <inheritdoc/>
        public override string CountryCode => CountryCodePrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new SaudiArabiaVatValidator().Validate(input);


    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => cleaned.Length == 15;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => VatRegex().IsMatch(cleaned);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned)
    {
        return ChecksumHelper.ValidateLuhn(cleaned)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidSaudiArabiaVatChecksum);
    }

    /// <summary>
    /// Static validation method for Saudi Arabian VAT numbers.
    /// </summary>
        public static ValidationResult ValidateVat(string? vat) => new SaudiArabiaVatValidator().Validate(vat);

    /// <summary>
    /// Gets details for a Saudi Arabian VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat) => new SaudiArabiaVatValidator().Parse(vat);
}