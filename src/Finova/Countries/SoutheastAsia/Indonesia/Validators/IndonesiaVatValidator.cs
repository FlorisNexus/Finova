using Finova.Core.Common;
using Finova.Core.Vat;
using Finova.Countries.SoutheastAsia.Indonesia.Validators;

namespace Finova.Countries.SoutheastAsia.Indonesia.Validators;

/// <summary>
/// Validates Indonesia VAT (PPN) number.
/// Reuses the NPWP (Tax ID) validator as the VAT number is the NPWP.
/// </summary>
public class IndonesiaVatValidator : IVatValidator
{
    public string CountryCode => "ID";

    public ValidationResult Validate(string? vat)
    {
        return IndonesiaNpwpValidator.ValidateStatic(vat);
    }

    public VatDetails? Parse(string? vat)
    {
        if (!Validate(vat).IsValid) return null;
        
        // NPWP Parser returns cleaned string
        var npwp = new IndonesiaNpwpValidator().Parse(vat);
        
        return new VatDetails
        {
            VatNumber = npwp ?? vat!,
            CountryCode = "ID",
            IsValid = true,
            IdentifierKind = "NPWP",
            IsEuVat = false,
            IsViesEligible = false,
            Notes = "Indonesia VAT (PPN) uses the NPWP number."
        };
    }
}
