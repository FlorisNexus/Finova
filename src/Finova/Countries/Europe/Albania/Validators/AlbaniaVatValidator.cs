using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Albania.Validators;

/// <summary>
/// Validator for Albanian VAT numbers (NIPT).
/// Format: 10 chars. [A-Z] + 8 digits + [A-Z].
/// </summary>
public partial class AlbaniaVatValidator : VatValidatorBase
{
    private const string CountryCodePrefix = "AL";
    private const int VatLength = 10;

    // Strict Regex: 1 Letter + 8 Digits + 1 Letter
    [GeneratedRegex(@"^[A-Z]\d{8}[A-Z]$")]
    private static partial Regex AlbaniaVatRegex();

    /// <inheritdoc/>
        public override string CountryCode => CountryCodePrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new AlbaniaVatValidator().Validate(input);


    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => cleaned.Length == VatLength;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => AlbaniaVatRegex().IsMatch(cleaned);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned)
    {
        // Checksum algorithm is not publicly standardized enough for safe validation.
        return ValidationResult.Success();
    }

    /// <summary>
    /// Static validation method for Albanian VAT numbers.
    /// </summary>
    /// <param name="vat">The VAT number to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
        public static ValidationResult ValidateVat(string? vat) => new AlbaniaVatValidator().Validate(vat);

    /// <summary>
    /// Gets details for an Albanian VAT number.
    /// </summary>
    /// <param name="vat">The VAT number to parse.</param>
    /// <returns>A <see cref="VatDetails"/> object or null if invalid.</returns>
    public static VatDetails? GetVatDetails(string? vat) => new AlbaniaVatValidator().Parse(vat);
}
