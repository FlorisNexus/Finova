using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.MiddleEast.Bahrain.Validators;

/// <summary>
/// Validator for Bahrain VAT Number / Tax Registration Number (TRN).
/// Format: 15 digits. Usually starts with 3.
/// </summary>
public partial class BahrainVatValidator : VatValidatorBase
{
    [GeneratedRegex(@"^3\d{14}$")]
    private static partial Regex VatRegex();

    private const string CountryCodePrefix = "BH";

    /// <inheritdoc/>
        public override string CountryCode => CountryCodePrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new BahrainVatValidator().Validate(input);


    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => cleaned.Length == 15;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => VatRegex().IsMatch(cleaned);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned)
    {
        // No public checksum algorithm available for BH.
        return ValidationResult.Success();
    }

    /// <summary>
    /// Static validation method for Bahraini VAT numbers.
    /// </summary>
        public static ValidationResult ValidateVat(string? vat) => new BahrainVatValidator().Validate(vat);

    /// <summary>
    /// Gets details for a Bahraini VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat) => new BahrainVatValidator().Parse(vat);
}
