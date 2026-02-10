using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.MiddleEast.Jordan.Validators;

/// <summary>
/// Validator for Jordanian IBANs.
/// Jordan IBAN format: JO + 2 check digits + 4 letters (bank code) + 4 digits (branch) + 18 alphanumeric (account)
/// </summary>
public class JordanIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "JO";

    /// <inheritdoc/>
    protected override int ExpectedLength => 30;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
    {
        return JordanBbanValidator.Validate(bban);
    }
}
