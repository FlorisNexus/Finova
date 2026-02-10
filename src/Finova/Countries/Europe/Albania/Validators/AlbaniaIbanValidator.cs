using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.Albania.Validators;

/// <summary>
/// Validator for Albanian IBANs.
/// </summary>
public class AlbaniaIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "AL";

    /// <inheritdoc/>
    protected override int ExpectedLength => 28;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
    {
        return AlbaniaBbanValidator.Validate(bban);
    }
}
