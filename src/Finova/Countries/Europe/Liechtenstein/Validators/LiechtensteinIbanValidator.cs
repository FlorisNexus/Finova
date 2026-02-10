using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.Liechtenstein.Validators;

/// <summary>
/// Validator for Liechtenstein IBANs.
/// </summary>
public class LiechtensteinIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "LI";

    /// <inheritdoc/>
    protected override int ExpectedLength => 21;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
        => LiechtensteinBbanValidator.Validate(bban);
}
