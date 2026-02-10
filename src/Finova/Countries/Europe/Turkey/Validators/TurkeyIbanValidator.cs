using Finova.Core.Common;
using Finova.Core.Iban;



namespace Finova.Countries.Europe.Turkey.Validators;

/// <summary>
/// Validator for Turkish IBANs.
/// </summary>
public class TurkeyIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "TR";

    /// <inheritdoc/>
    protected override int ExpectedLength => 26;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
    {
        return TurkeyBbanValidator.Validate(bban);
    }
}

