using Finova.Core.Common;
using Finova.Core.Vat;
using Finova.Countries.Africa.Egypt.Validators;

namespace Finova.Countries.Africa.Egypt.Validators;

/// <summary>
/// Validates Egypt VAT number.
/// Reuses the TRN (Tax Registration Number) validator.
/// </summary>
public class EgyptVatValidator : IVatValidator
{
    public string CountryCode => "EG";

    public ValidationResult Validate(string? vat)
    {
        return new EgyptTaxRegistrationNumberValidator().Validate(vat);
    }

    public VatDetails? Parse(string? vat)
    {
        if (!Validate(vat).IsValid) return null;
        
        var trn = new EgyptTaxRegistrationNumberValidator().Parse(vat);
        
        return new VatDetails
        {
            VatNumber = trn ?? vat!,
            CountryCode = "EG",
            IsValid = true,
            IdentifierKind = "TRN",
            IsEuVat = false,
            IsViesEligible = false,
            Notes = "Egypt VAT uses the Tax Registration Number (TRN)."
        };
    }
}
