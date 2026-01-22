using Finova.Core.Common;
using Finova.Core.Vat;
using Finova.Countries.SoutheastAsia.Vietnam.Validators;

namespace Finova.Countries.SoutheastAsia.Vietnam.Validators;

/// <summary>
/// Validates Vietnam VAT (GTGT) number.
/// Reuses the MST (Tax Code) validator.
/// </summary>
public class VietnamVatValidator : IVatValidator
{
    public string CountryCode => "VN";

    public ValidationResult Validate(string? vat)
    {
        return new VietnamTaxIdValidator().Validate(vat);
    }

    public VatDetails? Parse(string? vat)
    {
        if (!Validate(vat).IsValid) return null;
        
        var mst = new VietnamTaxIdValidator().Parse(vat);
        
        return new VatDetails
        {
            VatNumber = mst ?? vat!,
            CountryCode = "VN",
            IsValid = true,
            IdentifierKind = "MST",
            IsEuVat = false,
            IsViesEligible = false,
            Notes = "Vietnam VAT uses the Tax Code (MST)."
        };
    }
}
