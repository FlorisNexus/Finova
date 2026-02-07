using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.MiddleEast.UAE.Validators;

/// <summary>
/// Validator for United Arab Emirates Tax Registration Number (TRN).
/// Format: 15 digits starting with 100.
/// </summary>
public partial class UaeVatValidator : VatValidatorBase
{
    private const string CountryCodePrefix = "AE";

    [GeneratedRegex(@"^100\d{12}$")]
    private static partial Regex TrnPattern();

    /// <inheritdoc/>
        public override string CountryCode => CountryCodePrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new UaeVatValidator().Validate(input);


    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => cleaned.Length == 15;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => TrnPattern().IsMatch(cleaned);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned)
    {
        int sum = 0;
        for (int i = 0; i < cleaned.Length - 1; i++)
        {
            sum += cleaned[i] - '0';
        }

        int expectedCheck = (10 - (sum % 10)) % 10;
        int actualCheck = cleaned[^1] - '0';

        return expectedCheck == actualCheck
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidUaeTrnChecksum);
    }

    /// <summary>
    /// Static validation method for UAE VAT numbers.
    /// </summary>
        public static ValidationResult ValidateVat(string? vat) => new UaeVatValidator().Validate(vat);

    /// <summary>
    /// Gets details for a UAE VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat) => new UaeVatValidator().Parse(vat);
}