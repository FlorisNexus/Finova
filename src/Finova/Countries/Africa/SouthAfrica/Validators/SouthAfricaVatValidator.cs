using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Africa.SouthAfrica.Validators;

/// <summary>
/// Validator for South African VAT numbers.
/// Format: 10 digits starting with 4.
/// </summary>
public partial class SouthAfricaVatValidator : VatValidatorBase
{
    [GeneratedRegex(@"^4\d{9}$")]
    private static partial Regex VatRegex();

    private const string CountryCodePrefix = "ZA";

    /// <inheritdoc/>
        public override string CountryCode => CountryCodePrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new SouthAfricaVatValidator().Validate(input);


    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => cleaned.Length == 10;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => VatRegex().IsMatch(cleaned);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned)
    {
        return ChecksumHelper.ValidateLuhn(cleaned)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }

    /// <summary>
    /// Static validation method for South African VAT numbers.
    /// </summary>
        public static ValidationResult ValidateVat(string? vat) => new SouthAfricaVatValidator().Validate(vat);

    /// <summary>
    /// Gets details for a South African VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat) => new SouthAfricaVatValidator().Parse(vat);
}