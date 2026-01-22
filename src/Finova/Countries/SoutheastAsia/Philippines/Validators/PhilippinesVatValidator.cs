using Finova.Core.Common;
using Finova.Core.Vat;
using Finova.Countries.SoutheastAsia.Philippines.Validators;

namespace Finova.Countries.SoutheastAsia.Philippines.Validators;

/// <summary>
/// Validates Philippines VAT number.
/// Reuses the TIN validator.
/// </summary>
public class PhilippinesVatValidator : IVatValidator
{
    public string CountryCode => "PH";

    public ValidationResult Validate(string? vat)
    {
        return PhilippinesTinValidator.ValidateStatic(vat);
    }

    public VatDetails? Parse(string? vat)
    {
        if (!Validate(vat).IsValid) return null;
        
        var tinDetails = new PhilippinesTinValidator().Parse(vat);
        
        return new VatDetails
        {
            VatNumber = tinDetails?.VatNumber ?? vat!,
            CountryCode = "PH",
            IsValid = true,
            IdentifierKind = "TIN",
            IsEuVat = false,
            IsViesEligible = false,
            Notes = "Philippines VAT uses the TIN."
        };
    }
}
