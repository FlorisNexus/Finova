using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.NorthMacedonia.Validators;

/// <summary>
/// Validator for North Macedonian VAT numbers (EDB).
/// Format: 13 digits.
/// </summary>
public partial class NorthMacedoniaVatValidator : VatValidatorBase
{
    [GeneratedRegex(@"^\d{13}$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "MK";
    private static readonly int[] Weights = { 7, 6, 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 };

    /// <inheritdoc/>
        public override string CountryCode => VatPrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new NorthMacedoniaVatValidator().Validate(input);


    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => cleaned.Length == 13;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => VatRegex().IsMatch(cleaned);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned)
    {
        int sum = ChecksumHelper.CalculateWeightedSum(cleaned[..12], Weights);
        int remainder = sum % 11;
        int checkDigit = 11 - remainder;

        if (checkDigit == 10)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidNorthMacedoniaVatChecksumForbidden);
        }
        if (checkDigit == 11)
        {
            checkDigit = 0;
        }

        if (checkDigit != (cleaned[12] - '0'))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidNorthMacedoniaVatChecksum);
        }

        return ValidationResult.Success();
    }

    /// <summary>
    /// Static validation method for North Macedonian VAT numbers.
    /// </summary>
        public static ValidationResult ValidateVat(string? vat) => new NorthMacedoniaVatValidator().Validate(vat);

    /// <summary>
    /// Gets details for a North Macedonian VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat) => new NorthMacedoniaVatValidator().Parse(vat);
}
