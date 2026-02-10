using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.Hungary.Validators;

public class HungaryIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "HU";

    /// <inheritdoc/>
    protected override int ExpectedLength => 28;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
        => HungaryBbanValidator.Validate(bban);
}
