using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.BosniaAndHerzegovina.Validators;

/// <summary>
/// Validator for Bosnia and Herzegovina IBANs.
/// </summary>
public class BosniaAndHerzegovinaIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "BA";

    /// <inheritdoc/>
    protected override int ExpectedLength => 20;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
        => BosniaAndHerzegovinaBbanValidator.Validate(bban);
}
