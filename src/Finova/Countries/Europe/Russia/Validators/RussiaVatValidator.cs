using Finova.Core.Common;
using Finova.Core.Vat;
using Finova.Countries.Europe.Russia.Validators;

namespace Finova.Countries.Europe.Russia.Validators;

/// <summary>
/// Validates Russia VAT (NDS) number.
/// Reuses the INN (Tax ID) validator.
/// </summary>
public class RussiaVatValidator : IVatValidator
{
    public string CountryCode => "RU";

    public ValidationResult Validate(string? vat)
    {
        return new RussiaInnValidator().Validate(vat);
    }

    public VatDetails? Parse(string? vat)
    {
        if (!Validate(vat).IsValid) return null;
        
        var inn = new RussiaInnValidator().Parse(vat);
        
        return new VatDetails
        {
            VatNumber = inn ?? vat!,
            CountryCode = "RU",
            IsValid = true,
            IdentifierKind = "INN",
            IsEuVat = false,
            IsViesEligible = false,
            Notes = "Russia VAT (NDS) uses the INN number."
        };
    }
}
