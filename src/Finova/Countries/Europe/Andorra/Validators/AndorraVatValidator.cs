using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Andorra.Validators;

/// <summary>
/// Validator for Andorran VAT numbers (NRT - Número de Registre Tributari).
/// Format: 8 characters. [A-Z] + 6 digits + [A-Z].
/// </summary>
public partial class AndorraVatValidator : VatValidatorBase
{
    private const string CountryCodePrefix = "AD";
    private const int VatLength = 8;

    [GeneratedRegex(@"^[A-Z]\d{6}[A-Z]$")]
    private static partial Regex AndorraVatRegex();

    /// <inheritdoc/>
        public override string CountryCode => CountryCodePrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new AndorraVatValidator().Validate(input);


    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => cleaned.Length == VatLength;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => AndorraVatRegex().IsMatch(cleaned);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned)
    {
        // No public checksum algorithm available.
        return ValidationResult.Success();
    }

    /// <summary>
    /// Static validation method for Andorran VAT numbers.
    /// </summary>
    /// <param name="vat">The VAT number to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
        public static ValidationResult ValidateVat(string? vat) => new AndorraVatValidator().Validate(vat);

    /// <summary>
    /// Gets details for an Andorran VAT number.
    /// </summary>
    /// <param name="vat">The VAT number to parse.</param>
    /// <returns>A <see cref="VatDetails"/> object or null if invalid.</returns>
    public static VatDetails? GetVatDetails(string? vat) => new AndorraVatValidator().Parse(vat);
}