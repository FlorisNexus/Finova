using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.SanMarino.Validators;

public class SanMarinoIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "SM";

    /// <inheritdoc/>
    protected override int ExpectedLength => 27;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
        => SanMarinoBbanValidator.Validate(bban);
}
