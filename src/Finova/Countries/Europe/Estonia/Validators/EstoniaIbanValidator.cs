using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.Estonia.Validators;

/// <summary>
/// Validator for Estonian IBANs.
/// </summary>
public class EstoniaIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "EE";

    /// <inheritdoc/>
    protected override int ExpectedLength => 20;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
    {
        return EstoniaBbanValidator.Validate(bban);
    }
}
