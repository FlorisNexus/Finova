using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Germany.Validators;

/// <summary>
/// Validator for German VAT numbers.
/// </summary>
public partial class GermanyVatValidator : VatValidatorBase
{
    [GeneratedRegex(@"^\d{9}$")]
    private static partial Regex VatRegex();

    /// <inheritdoc/>
        public override string CountryCode => "DE";
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new GermanyVatValidator().Validate(input);


    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => cleaned.Length == 9;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => VatRegex().IsMatch(cleaned);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned)
    {
        return ChecksumHelper.ValidateISO7064Mod11_10(cleaned)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }

    /// <summary>
    /// Static validation method for German VAT numbers.
    /// </summary>
    /// <param name="vat">The VAT number to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
        public static ValidationResult ValidateVat(string? vat) => new GermanyVatValidator().Validate(vat);
}