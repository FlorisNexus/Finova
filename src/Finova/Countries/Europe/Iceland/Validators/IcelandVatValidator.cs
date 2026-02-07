using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Iceland.Validators;

/// <summary>
/// Validator for Icelandic VAT numbers (VSK-númer).
/// Format: 5, 6, or 10 digits.
/// </summary>
public partial class IcelandVatValidator : VatValidatorBase
{
    [GeneratedRegex(@"^(\d{5,6}|\d{10})$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "IS";

    /// <inheritdoc/>
        public override string CountryCode => VatPrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new IcelandVatValidator().Validate(input);


    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => cleaned.Length == 5 || cleaned.Length == 6 || cleaned.Length == 10;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => VatRegex().IsMatch(cleaned);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned)
    {
        // No public checksum algorithm available.
        return ValidationResult.Success();
    }

    /// <summary>
    /// Static validation method for Icelandic VAT numbers.
    /// </summary>
        public static ValidationResult ValidateVat(string? vat) => new IcelandVatValidator().Validate(vat);

    /// <summary>
    /// Gets details for an Icelandic VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat) => new IcelandVatValidator().Parse(vat);
}