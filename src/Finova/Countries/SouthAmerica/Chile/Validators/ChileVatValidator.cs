using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.SouthAmerica.Chile.Validators;

/// <summary>
/// Validator for Chilean VAT identifier (RUT).
/// Format: 8 or 9 characters.
/// </summary>
public partial class ChileVatValidator : VatValidatorBase
{
    private const string CountryCodePrefix = "CL";

    /// <inheritdoc/>
        public override string CountryCode => CountryCodePrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new ChileVatValidator().Validate(input);


    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => cleaned.Length == 8 || cleaned.Length == 9;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => true; // Handled by RUT validator

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned)
    {
        return ChileRutValidator.ValidateStatic(cleaned);
    }

    /// <inheritdoc/>
    protected override VatDetails CreateDetails(string cleaned)
    {
        return new VatDetails
        {
            VatNumber = cleaned,
            CountryCode = CountryCodePrefix,
            IsValid = true,
            IdentifierKind = "RUT",
            IsEuVat = false,
            IsViesEligible = false,
            Notes = "Chilean tax identifier (RUT)"
        };
    }

    /// <summary>
    /// Static validation method for Chilean VAT numbers.
    /// </summary>
        public static ValidationResult ValidateVat(string? vat) => new ChileVatValidator().Validate(vat);

    /// <summary>
    /// Gets details for a Chilean VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat) => new ChileVatValidator().Parse(vat);
}