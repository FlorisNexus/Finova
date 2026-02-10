using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.Croatia.Validators;

public class CroatiaIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "HR";

    /// <inheritdoc/>
    protected override int ExpectedLength => 21;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
        => CroatiaBbanValidator.Validate(bban);
}
