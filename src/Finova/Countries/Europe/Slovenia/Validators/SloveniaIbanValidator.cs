using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.Slovenia.Validators;

public class SloveniaIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "SI";

    /// <inheritdoc/>
    protected override int ExpectedLength => 19;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
        => SloveniaBbanValidator.Validate(bban);
}
