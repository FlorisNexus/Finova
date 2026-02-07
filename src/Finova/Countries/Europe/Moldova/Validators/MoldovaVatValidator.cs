using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Moldova.Validators;

/// <summary>
/// Validator for Moldovan VAT numbers (IDNO).
/// Format: 13 digits.
/// </summary>
public partial class MoldovaVatValidator : VatValidatorBase
{
    [GeneratedRegex(@"^\d{13}$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "MD";

    /// <inheritdoc/>
        public override string CountryCode => VatPrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new MoldovaVatValidator().Validate(input);


    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => cleaned.Length == 13;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => VatRegex().IsMatch(cleaned);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned)
    {
        // No public checksum algorithm available for MD.
        return ValidationResult.Success();
    }

    /// <summary>
    /// Static validation method for Moldovan VAT numbers.
    /// </summary>
        public static ValidationResult ValidateVat(string? vat) => new MoldovaVatValidator().Validate(vat);

    /// <summary>
    /// Gets details for a Moldovan VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat) => new MoldovaVatValidator().Parse(vat);
}