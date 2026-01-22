using Finova.Core.Common;
using Finova.Core.Vat;
using Finova.Countries.Europe.Turkey.Validators;

namespace Finova.Countries.Europe.Turkey.Validators;

/// <summary>
/// Validates Turkey VAT (KDV) number.
/// Reuses the VKN (Tax ID) validator.
/// </summary>
public class TurkeyVatValidator : IVatValidator
{
    public string CountryCode => "TR";

    public ValidationResult Validate(string? vat)
    {
        return new TurkeyVknValidator().Validate(vat);
    }

    public VatDetails? Parse(string? vat)
    {
        if (!Validate(vat).IsValid) return null;
        
        var vknDetails = new TurkeyVknValidator().Parse(vat);
        
        return new VatDetails
        {
            VatNumber = vknDetails?.VatNumber ?? vat!,
            CountryCode = "TR",
            IsValid = true,
            IdentifierKind = "VKN",
            IsEuVat = false,
            IsViesEligible = false,
            Notes = "Turkey VAT (KDV) uses the VKN number."
        };
    }
}
