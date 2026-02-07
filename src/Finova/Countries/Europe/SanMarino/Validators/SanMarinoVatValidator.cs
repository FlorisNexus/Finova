using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.SanMarino.Validators;

/// <summary>
/// Validator for San Marino VAT numbers (Codice Operatore Economico).
/// Format: 5 digits.
/// </summary>
public partial class SanMarinoVatValidator : VatValidatorBase
{
    [GeneratedRegex(@"^\d{5}$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "SM";

    /// <inheritdoc/>
        public override string CountryCode => VatPrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new SanMarinoVatValidator().Validate(input);


    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => cleaned.Length == 5;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => VatRegex().IsMatch(cleaned);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned)
    {
        // No public checksum algorithm available for SM.
        return ValidationResult.Success();
    }

    /// <summary>
    /// Static validation method for San Marino VAT numbers.
    /// </summary>
        public static ValidationResult ValidateVat(string? vat) => new SanMarinoVatValidator().Validate(vat);

    /// <summary>
    /// Gets details for a San Marino VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat) => new SanMarinoVatValidator().Parse(vat);
}
