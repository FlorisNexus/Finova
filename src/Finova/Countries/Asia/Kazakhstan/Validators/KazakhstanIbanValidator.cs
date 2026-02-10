using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Asia.Kazakhstan.Validators;

/// <summary>
/// Validator for Kazakhstan IBANs.
/// Kazakhstan IBAN format: KZ + 2 check digits + 3 digits (bank code) + 13 alphanumeric (account)
/// </summary>
public class KazakhstanIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "KZ";

    /// <inheritdoc/>
    protected override int ExpectedLength => 20;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
    {
        return KazakhstanBbanValidator.Validate(bban);
    }
}
