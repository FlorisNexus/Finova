using Finova.Core.Common;
using Finova.Core.Iban;



namespace Finova.Countries.Europe.Greenland.Validators;

/// <summary>
/// Validator for Greenland IBANs.
/// </summary>
public class GreenlandIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "GL";

    /// <inheritdoc/>
    protected override int ExpectedLength => 18;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
    {
        return GreenlandBbanValidator.Validate(bban);
    }
}

