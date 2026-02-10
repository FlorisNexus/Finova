using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.Denmark.Validators;

/// <summary>
/// Validator for Danish IBANs.
/// </summary>
public class DenmarkIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "DK";

    /// <inheritdoc/>
    protected override int ExpectedLength => 18;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
    {
        return DenmarkBbanValidator.Validate(bban);
    }
}
