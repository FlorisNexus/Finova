using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Africa.Mozambique.Validators;

/// <summary>
/// Validator for Mozambique IBANs.
/// Mozambique IBAN format: MZ + 2 check digits + 21 digits BBAN.
/// Length: 25 characters.
/// </summary>
public class MozambiqueIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "MZ";

    /// <inheritdoc/>
    protected override int ExpectedLength => 25;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
        => MozambiqueBbanValidator.Validate(bban);
}
