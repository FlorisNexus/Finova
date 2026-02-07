using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Asia.China.Validators;

/// <summary>
/// Validator for Chinese VAT identifier (Unified Social Credit Code - USCC).
/// Format: 18 alphanumeric characters.
/// </summary>
public partial class ChinaVatValidator : VatValidatorBase
{
    private const string CountryCodePrefix = "CN";

    /// <inheritdoc/>
        public override string CountryCode => CountryCodePrefix;
    /// <summary>
    /// Static validation method for tests.
    /// </summary>
    public static ValidationResult ValidateStatic(string? input) => new ChinaVatValidator().Validate(input);


    /// <inheritdoc/>
    protected override bool IsValidLength(string cleaned) => cleaned.Length == 18;

    /// <inheritdoc/>
    protected override bool ValidateFormat(string cleaned) => true; // Handled by USCC validator

    /// <inheritdoc/>
    protected override ValidationResult ValidateChecksum(string cleaned)
    {
        return ChinaUnifiedSocialCreditCodeValidator.ValidateUscc(cleaned);
    }

    /// <inheritdoc/>
    protected override VatDetails CreateDetails(string cleaned)
    {
        // First character indicates registration authority
        // Second character indicates entity type
        var entityType = cleaned[1] switch
        {
            '1' => "Institution (机构)",
            '2' => "Foreign-invested enterprise (外商投资企业)",
            '3' => "Enterprise (企业)",
            '4' => "Social organization (社会组织)",
            '5' => "Civilian-run non-enterprise unit (民办非企业单位)",
            '6' => "Foundation (基金会)",
            '7' => "Sole proprietorship (个体工商户)",
            '9' => "Other organization (其他组织)",
            _ => "Unknown"
        };

        return new VatDetails
        {
            VatNumber = cleaned,
            CountryCode = CountryCodePrefix,
            IsValid = true,
            IdentifierKind = "USCC",
            IsEuVat = false,
            IsViesEligible = false,
            Notes = $"Chinese VAT registration - {entityType}"
        };
    }

    /// <summary>
    /// Static validation method for Chinese VAT numbers.
    /// </summary>
        public static ValidationResult ValidateVat(string? vat) => new ChinaVatValidator().Validate(vat);

    /// <summary>
    /// Gets details for a Chinese VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat) => new ChinaVatValidator().Parse(vat);
}