using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Serbia.Validators;

/// <summary>
/// Validator for Serbia VAT numbers (PIB).
/// Format: 9 digits.
/// </summary>
public partial class SerbiaVatValidator : VatValidatorBase
{
    [GeneratedRegex(@"^\d{9}$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "RS";

    /// <inheritdoc/>
        public override string CountryCode => VatPrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new SerbiaVatValidator().Validate(input);


    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => cleaned.Length == 9;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => VatRegex().IsMatch(cleaned);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned)
    {
        return ChecksumHelper.ValidateISO7064Mod11_10(cleaned)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidSerbiaVatChecksum);
    }

    /// <summary>
    /// Static validation method for Serbian VAT numbers.
    /// </summary>
        public static ValidationResult ValidateVat(string? vat) => new SerbiaVatValidator().Validate(vat);

    /// <summary>
    /// Gets details for a Serbian VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat) => new SerbiaVatValidator().Parse(vat);
}
