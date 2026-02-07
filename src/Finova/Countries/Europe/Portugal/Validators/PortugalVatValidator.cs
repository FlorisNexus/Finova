using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Portugal.Validators;

/// <summary>
/// Validator for Portuguese VAT numbers (NIPC).
/// Format: 9 digits.
/// </summary>
public partial class PortugalVatValidator : VatValidatorBase
{
    [GeneratedRegex(@"^\d{9}$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "PT";

    /// <inheritdoc/>
        public override string CountryCode => VatPrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new PortugalVatValidator().Validate(input);


    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => cleaned.Length == 9;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => VatRegex().IsMatch(cleaned);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned)
    {
        int[] weights = { 9, 8, 7, 6, 5, 4, 3, 2 };

        int remainder = ChecksumHelper.CalculateWeightedModulo11(cleaned.Substring(0, 8), weights);

        int checkDigit = 11 - remainder;
        if (checkDigit > 9)
        {
            checkDigit = 0;
        }

        int lastDigit = cleaned[8] - '0';
        return checkDigit == lastDigit
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, string.Format(ValidationMessages.InvalidVatChecksum, "Portugal"));
    }

    /// <summary>
    /// Static validation method for Portuguese VAT numbers.
    /// </summary>
        public static ValidationResult ValidateVat(string? vat) => new PortugalVatValidator().Validate(vat);

    /// <summary>
    /// Gets details for a Portuguese VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat) => new PortugalVatValidator().Parse(vat);
}