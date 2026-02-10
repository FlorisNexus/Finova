using Finova.Core.Common;
using Finova.Core.Iban;



namespace Finova.Countries.Europe.Azerbaijan.Validators;

/// <summary>
/// Validator for Azerbaijan IBANs.
/// </summary>
public class AzerbaijanIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "AZ";

    /// <inheritdoc/>
    protected override int ExpectedLength => 28;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
    {
        return AzerbaijanBbanValidator.Validate(bban);
    }
}

