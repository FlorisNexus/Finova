using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Georgia.Validators;

/// <summary>
/// Validator for Georgian VAT numbers.
/// Format: 9 digits.
/// </summary>
public partial class GeorgiaVatValidator : VatValidatorBase
{
    [GeneratedRegex(@"^\d{9}$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "GE";

    /// <inheritdoc/>
        public override string CountryCode => VatPrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new GeorgiaVatValidator().Validate(input);


    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => cleaned.Length == 9;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => VatRegex().IsMatch(cleaned);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned)
    {
        // No public checksum algorithm available.
        return ValidationResult.Success();
    }

    /// <summary>
    /// Static validation method for Georgian VAT numbers.
    /// </summary>
        public static ValidationResult ValidateVat(string? vat) => new GeorgiaVatValidator().Validate(vat);

    /// <summary>
    /// Gets details for a Georgian VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat) => new GeorgiaVatValidator().Parse(vat);
}