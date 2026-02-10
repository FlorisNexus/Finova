using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.Luxembourg.Validators;

/// <summary>
/// Validator for Luxembourger IBANs.
/// </summary>
public class LuxembourgIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "LU";

    /// <inheritdoc/>
    protected override int ExpectedLength => 20;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
    {
        return LuxembourgBbanValidator.Validate(bban);
    }
}
