using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.NorthAmerica.ElSalvador.Validators;

/// <summary>
/// Validator for El Salvadoran IBANs.
/// El Salvador IBAN format: SV + 2 check digits + 4 letters (bank code) + 20 digits (account)
/// </summary>
public class ElSalvadorIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "SV";

    /// <inheritdoc/>
    protected override int ExpectedLength => 28;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
    {
        return ElSalvadorBbanValidator.Validate(bban);
    }
}
