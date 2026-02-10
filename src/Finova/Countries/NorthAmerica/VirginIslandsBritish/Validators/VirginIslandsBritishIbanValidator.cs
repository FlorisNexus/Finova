using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.NorthAmerica.VirginIslandsBritish.Validators;

/// <summary>
/// Validator for British Virgin Islands IBANs.
/// British Virgin Islands IBAN format: VG + 2 check digits + 4 letters (bank code) + 16 digits (account)
/// </summary>
public class VirginIslandsBritishIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "VG";

    /// <inheritdoc/>
    protected override int ExpectedLength => 24;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
    {
        return VirginIslandsBritishBbanValidator.Validate(bban);
    }
}
