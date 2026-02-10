using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.Portugal.Validators;

/// <summary>
/// Validator for Portuguese IBANs.
/// </summary>
public class PortugalIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "PT";

    /// <inheritdoc/>
    protected override int ExpectedLength => 25;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
    {
        return PortugalBbanValidator.Validate(bban);
    }
}
