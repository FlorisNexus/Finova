using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.SouthAmerica.Mexico.Validators;

/// <summary>
/// Validator for Mexican VAT identifier (RFC).
/// Format: 12 or 13 characters.
/// </summary>
public partial class MexicoVatValidator : VatValidatorBase
{
    private const string CountryCodePrefix = "MX";

    /// <inheritdoc/>
        public override string CountryCode => CountryCodePrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new MexicoVatValidator().Validate(input);


    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => cleaned.Length == 12 || cleaned.Length == 13;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => true; // Handled by RFC validator

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned)
    {
        return MexicoRfcValidator.ValidateStatic(cleaned);
    }

    /// <inheritdoc/>
    protected override VatDetails CreateDetails(string cleaned)
    {
        var entityType = cleaned.Length == 12 ? "Company" : "Individual";

        return new VatDetails
        {
            VatNumber = cleaned,
            CountryCode = CountryCodePrefix,
            IsValid = true,
            IdentifierKind = "RFC",
            IsEuVat = false,
            IsViesEligible = false,
            Notes = $"Mexican tax identifier (RFC) - {entityType}"
        };
    }

    /// <summary>
    /// Static validation method for Mexican VAT numbers.
    /// </summary>
        public static ValidationResult ValidateVat(string? vat) => new MexicoVatValidator().Validate(vat);

    /// <summary>
    /// Gets details for a Mexican VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat) => new MexicoVatValidator().Parse(vat);
}