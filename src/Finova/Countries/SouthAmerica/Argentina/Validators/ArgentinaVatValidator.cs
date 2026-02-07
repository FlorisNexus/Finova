using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.SouthAmerica.Argentina.Validators;

/// <summary>
/// Validator for Argentine VAT identifier (CUIT).
/// Format: 11 digits.
/// </summary>
public partial class ArgentinaVatValidator : VatValidatorBase
{
    private const string CountryCodePrefix = "AR";

    /// <inheritdoc/>
        public override string CountryCode => CountryCodePrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new ArgentinaVatValidator().Validate(input);


    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => cleaned.Length == 11;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => cleaned.All(char.IsDigit);

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned)
    {
        return ArgentinaCuitValidator.ValidateStatic(cleaned);
    }

    /// <inheritdoc/>
    protected override VatDetails CreateDetails(string cleaned)
    {
        // First two digits indicate entity type
        var prefix = cleaned[..2];
        var entityType = prefix switch
        {
            "20" or "23" or "24" or "27" => "Individual",
            "30" or "33" or "34" => "Company",
            _ => "Unknown"
        };

        return new VatDetails
        {
            VatNumber = cleaned,
            CountryCode = CountryCodePrefix,
            IsValid = true,
            IdentifierKind = "CUIT",
            IsEuVat = false,
            IsViesEligible = false,
            Notes = $"Argentine tax identifier (CUIT) - {entityType}"
        };
    }

    /// <summary>
    /// Static validation method for Argentine VAT numbers.
    /// </summary>
        public static ValidationResult ValidateVat(string? vat) => new ArgentinaVatValidator().Validate(vat);

    /// <summary>
    /// Gets details for an Argentine VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat) => new ArgentinaVatValidator().Parse(vat);
}