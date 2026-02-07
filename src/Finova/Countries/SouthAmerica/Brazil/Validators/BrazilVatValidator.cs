using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.SouthAmerica.Brazil.Validators;

/// <summary>
/// Validator for Brazilian VAT identifier (CNPJ).
/// Format: 14 digits.
/// </summary>
public partial class BrazilVatValidator : VatValidatorBase
{
    private const string CountryCodePrefix = "BR";

    /// <inheritdoc/>
        public override string CountryCode => CountryCodePrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new BrazilVatValidator().Validate(input);


    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => cleaned.Length == 14;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => cleaned.All(char.IsDigit);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned)
    {
        return BrazilCnpjValidator.ValidateCnpj(cleaned);
    }

    /// <inheritdoc/>
    protected override VatDetails CreateDetails(string cleaned)
    {
        return new VatDetails
        {
            VatNumber = cleaned,
            CountryCode = CountryCodePrefix,
            IsValid = true,
            IdentifierKind = "CNPJ",
            IsEuVat = false,
            IsViesEligible = false,
            Notes = "Brazilian business tax identifier (CNPJ)"
        };
    }

    /// <summary>
    /// Static validation method for Brazilian VAT numbers.
    /// </summary>
        public static ValidationResult ValidateVat(string? vat) => new BrazilVatValidator().Validate(vat);

    /// <summary>
    /// Gets details for a Brazilian VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat) => new BrazilVatValidator().Parse(vat);
}