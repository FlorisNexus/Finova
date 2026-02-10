using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.Greece.Validators;

/// <summary>
/// Validator for Greek IBANs.
/// </summary>
public class GreeceIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "GR";

    /// <inheritdoc/>
    protected override int ExpectedLength => 27;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
    {
        return GreeceBbanValidator.Validate(bban);
    }
}
