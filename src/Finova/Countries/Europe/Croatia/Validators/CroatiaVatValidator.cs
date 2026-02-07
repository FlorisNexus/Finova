using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Croatia.Validators;

/// <summary>
/// Validator for Croatian VAT numbers (OIB).
/// Format: 11 digits.
/// </summary>
public partial class CroatiaVatValidator : VatValidatorBase
{
    [GeneratedRegex(@"^\d{11}$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "HR";

    /// <inheritdoc/>
        public override string CountryCode => VatPrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new CroatiaVatValidator().Validate(input);


    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => cleaned.Length == 11;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => VatRegex().IsMatch(cleaned);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned)
    {
        return ChecksumHelper.ValidateISO7064Mod11_10(cleaned)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidCroatiaVatChecksum);
    }

    /// <summary>
    /// Static validation method for Croatian VAT numbers.
    /// </summary>
    /// <param name="vat">The VAT number to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
        public static ValidationResult ValidateVat(string? vat) => new CroatiaVatValidator().Validate(vat);

    /// <summary>
    /// Gets details for a Croatian VAT number.
    /// </summary>
    /// <param name="vat">The VAT number to parse.</param>
    /// <returns>A <see cref="VatDetails"/> object or null if invalid.</returns>
    public static VatDetails? GetVatDetails(string? vat) => new CroatiaVatValidator().Parse(vat);
}
