using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.MiddleEast.Bahrain.Validators;

/// <summary>
/// Validator for Bahraini IBANs.
/// Bahrain IBAN format: BH + 2 check digits + 4 letters (bank code) + 14 alphanumeric (account)
/// </summary>
public class BahrainIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "BH";

    /// <inheritdoc/>
    protected override int ExpectedLength => 22;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
    {
        return BahrainBbanValidator.Validate(bban);
    }
}
