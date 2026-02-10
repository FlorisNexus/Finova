using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.Poland.Validators;

public class PolandIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "PL";

    /// <inheritdoc/>
    protected override int ExpectedLength => 28;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
        => PolandBbanValidator.Validate(bban);
}
