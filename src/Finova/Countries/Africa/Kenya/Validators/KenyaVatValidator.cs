using Finova.Core.Common;
using Finova.Core.Vat;
using Finova.Countries.Africa.Kenya.Validators;

namespace Finova.Countries.Africa.Kenya.Validators;

/// <summary>
/// Validates Kenya VAT number.
/// Reuses the KRA PIN validator.
/// </summary>
public class KenyaVatValidator : IVatValidator
{
    public string CountryCode => "KE";

    public ValidationResult Validate(string? vat)
    {
        return KenyaPinValidator.ValidateStatic(vat);
    }

    public VatDetails? Parse(string? vat)
    {
        if (!Validate(vat).IsValid) return null;
        
        var pin = new KenyaPinValidator().Parse(vat);
        
        return new VatDetails
        {
            VatNumber = pin ?? vat!,
            CountryCode = "KE",
            IsValid = true,
            IdentifierKind = "PIN",
            IsEuVat = false,
            IsViesEligible = false,
            Notes = "Kenya VAT uses the KRA PIN."
        };
    }
}
