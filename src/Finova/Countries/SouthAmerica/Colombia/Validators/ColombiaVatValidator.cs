using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.SouthAmerica.Colombia.Validators;

/// <summary>
/// Validator for Colombian VAT identifier (NIT).
/// Format: 9 or 10 digits.
/// </summary>
public partial class ColombiaVatValidator : VatValidatorBase
{
    private const string CountryCodePrefix = "CO";

    [GeneratedRegex(@"^\d{9,10}$")]
    private static partial Regex NitPattern();

    /// <inheritdoc/>
        public override string CountryCode => CountryCodePrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new ColombiaVatValidator().Validate(input);


    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => cleaned.Length == 9 || cleaned.Length == 10;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => NitPattern().IsMatch(cleaned);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned)
    {
        // If 9 digits, no check digit validation (just the base number)
        if (cleaned.Length == 9)
        {
            return ValidationResult.Success();
        }

        // Validate checksum for 10 digit NIT
        // Weights: 41, 37, 29, 23, 19, 17, 13, 7, 3
        int[] weights = [41, 37, 29, 23, 19, 17, 13, 7, 3];
        int sum = 0;

        for (int i = 0; i < 9; i++)
        {
            sum += (cleaned[i] - '0') * weights[i];
        }

        int remainder = sum % 11;
        int checkDigit = remainder > 1 ? 11 - remainder : remainder;

        return checkDigit == (cleaned[9] - '0')
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidColombianNitChecksum);
    }

    /// <inheritdoc/>
    protected override VatDetails CreateDetails(string cleaned)
    {
        return new VatDetails
        {
            VatNumber = cleaned,
            CountryCode = CountryCodePrefix,
            IsValid = true,
            IdentifierKind = "NIT",
            IsEuVat = false,
            IsViesEligible = false,
            Notes = "Colombian tax identifier (NIT)"
        };
    }

    /// <summary>
    /// Static validation method for Colombian VAT numbers.
    /// </summary>
        public static ValidationResult ValidateVat(string? vat) => new ColombiaVatValidator().Validate(vat);

    /// <summary>
    /// Gets details for a Colombian VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat) => new ColombiaVatValidator().Parse(vat);
}