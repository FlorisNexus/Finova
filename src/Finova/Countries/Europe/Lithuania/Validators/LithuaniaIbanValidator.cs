using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.Lithuania.Validators;

/// <summary>
/// Validator for Lithuanian IBANs.
/// </summary>
public class LithuaniaIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "LT";

    /// <inheritdoc/>
    protected override int ExpectedLength => 20;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
    {
        return LithuaniaBbanValidator.Validate(bban);
    }
}
