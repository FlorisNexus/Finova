using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Azerbaijan.Validators;

/// <summary>
/// Validator for Azerbaijan VAT numbers (VÖEN).
/// Format: 10 digits.
/// </summary>
public partial class AzerbaijanVatValidator : VatValidatorBase
{
    private const string CountryCodePrefix = "AZ";
    private const int VatLength = 10;

    [GeneratedRegex(@"^\d{10}$")]
    private static partial Regex VatRegex();

    /// <inheritdoc/>
        public override string CountryCode => CountryCodePrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new AzerbaijanVatValidator().Validate(input);


    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => cleaned.Length == VatLength;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => VatRegex().IsMatch(cleaned);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned)
    {
        // No public checksum algorithm available for AZ.
        return ValidationResult.Success();
    }

    /// <summary>
    /// Static validation method for Azerbaijan VAT numbers.
    /// </summary>
        public static ValidationResult ValidateVat(string? vat) => new AzerbaijanVatValidator().Validate(vat);

    /// <summary>
    /// Gets details for an Azerbaijan VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat) => new AzerbaijanVatValidator().Parse(vat);
}