using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Ukraine.Validators;

/// <summary>
/// Validator for Ukrainian VAT numbers (IPN).
/// Format: 8, 9, or 12 digits.
/// </summary>
public partial class UkraineVatValidator : VatValidatorBase
{
    [GeneratedRegex(@"^(\d{8}|\d{9}|\d{12})$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "UA";

    /// <inheritdoc/>
        public override string CountryCode => VatPrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new UkraineVatValidator().Validate(input);


    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => cleaned.Length == 8 || cleaned.Length == 9 || cleaned.Length == 12;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => VatRegex().IsMatch(cleaned);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned)
    {
        // No public checksum algorithm available for UA.
        return ValidationResult.Success();
    }

    /// <summary>
    /// Static validation method for Ukrainian VAT numbers.
    /// </summary>
        public static ValidationResult ValidateVat(string? vat) => new UkraineVatValidator().Validate(vat);

    /// <summary>
    /// Gets details for a Ukrainian VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat) => new UkraineVatValidator().Parse(vat);
}