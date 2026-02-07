using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.FaroeIslands.Validators;

/// <summary>
/// Validator for Faroe Islands VAT numbers.
/// Format: 6 digits.
/// </summary>
public partial class FaroeIslandsVatValidator : VatValidatorBase
{
    [GeneratedRegex(@"^\d{6}$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "FO";

    /// <inheritdoc/>
        public override string CountryCode => VatPrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new FaroeIslandsVatValidator().Validate(input);


    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => cleaned.Length == 6;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => VatRegex().IsMatch(cleaned);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned)
    {
        // No public checksum algorithm available.
        return ValidationResult.Success();
    }

    /// <summary>
    /// Static validation method for Faroe Islands VAT numbers.
    /// </summary>
        public static ValidationResult ValidateVat(string? vat) => new FaroeIslandsVatValidator().Validate(vat);

    /// <summary>
    /// Gets details for a Faroe Islands VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat) => new FaroeIslandsVatValidator().Parse(vat);
}