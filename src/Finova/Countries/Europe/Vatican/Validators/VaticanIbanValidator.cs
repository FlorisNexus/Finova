using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.Vatican.Validators;

/// <summary>
/// Validator for Vatican IBANs.
/// </summary>
public class VaticanIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "VA";

    /// <inheritdoc/>
    protected override int ExpectedLength => 22;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
    {
        return VaticanBbanValidator.Validate(bban);
    }
}
