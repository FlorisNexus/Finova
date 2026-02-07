using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Asia.SouthKorea.Validators;

/// <summary>
/// Validator for South Korean Business Registration Number (BRN).
/// Format: 10 digits.
/// </summary>
public partial class SouthKoreaVatValidator : VatValidatorBase
{
    private const string CountryCodePrefix = "KR";

    [GeneratedRegex(@"^\d{10}$")]
    private static partial Regex BrnPattern();

    /// <inheritdoc/>
        public override string CountryCode => CountryCodePrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new SouthKoreaVatValidator().Validate(input);


    /// <inheritdoc/>
    protected override VatDetails CreateDetails(string cleaned)
    {
        return new VatDetails
        {
            CountryCode = CountryCode,
            VatNumber = cleaned,
            IsValid = true,
            IdentifierKind = "BRN",
            IsEuVat = false,
            Notes = "사업자등록번호 (Business Registration Number)"
        };
    }

    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => cleaned.Length == 10;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => BrnPattern().IsMatch(cleaned);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned)
    {
        // Weights: 1, 3, 7, 1, 3, 7, 1, 3, 5
        int[] weights = [1, 3, 7, 1, 3, 7, 1, 3, 5];
        int sum = 0;

        for (int i = 0; i < 9; i++)
        {
            sum += (cleaned[i] - '0') * weights[i];
        }

        // Add additional digit from position 8
        sum += ((cleaned[8] - '0') * 5) / 10;

        int checkDigit = (10 - (sum % 10)) % 10;

        return checkDigit == (cleaned[9] - '0')
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidSouthKoreaVatChecksum);
    }

    /// <summary>
    /// Static validation method for South Korean VAT numbers.
    /// </summary>
        public static ValidationResult ValidateVat(string? vat) => new SouthKoreaVatValidator().Validate(vat);

    /// <summary>
    /// Gets details for a South Korean VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat) => new SouthKoreaVatValidator().Parse(vat);
}