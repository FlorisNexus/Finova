using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Asia.TimorLeste.Validators;

/// <summary>
/// Validator for Timor-Leste (East Timor) IBANs.
/// Timor-Leste IBAN format: TL + 2 check digits + 3 digits (bank code) + 14 digits (account) + 2 digits (check)
/// </summary>
public class TimorLesteIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "TL";

    /// <inheritdoc/>
    protected override int ExpectedLength => 23;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
    {
        return TimorLesteBbanValidator.Validate(bban);
    }
}
