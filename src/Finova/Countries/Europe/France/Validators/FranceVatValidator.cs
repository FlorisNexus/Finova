using Finova.Core.Common;
using Finova.Core.Vat;
using Finova.Countries.Europe.France.Models;

namespace Finova.Countries.Europe.France.Validators;

/// <summary>
/// Validator for French VAT numbers.
/// </summary>
public class FranceVatValidator : VatValidatorBase
{
    private const string CountryCodePrefix = "FR";

    /// <inheritdoc/>
        public override string CountryCode => CountryCodePrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new FranceVatValidator().Validate(input);


    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => cleaned.Length == 11;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => long.TryParse(cleaned, out _);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned)
    {
        var keyStr = cleaned.Substring(0, 2);
        var sirenStr = cleaned.Substring(2, 9);

        if (!int.TryParse(keyStr, out int key))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidNumericFormat);
        }

        int sirenMod97 = ChecksumHelper.CalculateModulo97(sirenStr);
        if (sirenMod97 == -1)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidSirenFormat);
        }

        int calculatedKey = (12 + 3 * sirenMod97) % 97;

        if (key != calculatedKey)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksumKey);
        }

        if (!ChecksumHelper.ValidateLuhn(sirenStr))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksumSirenLuhn);
        }

        return ValidationResult.Success();
    }

    /// <inheritdoc/>
    protected override VatDetails CreateDetails(string cleaned)
    {
        var siren = cleaned.Substring(2, 9);
        return new FranceVatDetails
        {
            VatNumber = $"{CountryCodePrefix}{cleaned}",
            CountryCode = CountryCodePrefix,
            IsValid = true,
            Siren = siren
        };
    }

    /// <summary>
    /// Static validation method for French VAT numbers.
    /// </summary>
        public static ValidationResult ValidateVat(string? vat) => new FranceVatValidator().Validate(vat);

    /// <summary>
    /// Gets details for a French VAT number.
    /// </summary>
    public static FranceVatDetails? GetVatDetails(string? vat)
    {
        var validator = new FranceVatValidator();
        return (FranceVatDetails?)validator.Parse(vat);
    }
}
