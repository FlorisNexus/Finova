using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.Ukraine.Validators;

/// <summary>
/// Validator for Ukraine IBANs.
/// </summary>
public class UkraineIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "UA";

    /// <inheritdoc/>
    protected override int ExpectedLength => 29;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
        => UkraineBbanValidator.Validate(bban);
}
