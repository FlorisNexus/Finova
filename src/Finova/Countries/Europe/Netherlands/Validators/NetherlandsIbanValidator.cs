using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.Netherlands.Validators;

/// <summary>
/// Validator for Dutch (Netherlands) IBAN bank accounts.
/// Dutch IBAN format: NL + 2 check digits + 4 bank code + 10 account number (18 characters total).
/// </summary>
public class NetherlandsIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "NL";

    /// <inheritdoc/>
    protected override int ExpectedLength => 18;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
    {
        return NetherlandsBbanValidator.Validate(bban);
    }
}
