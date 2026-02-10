using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.Moldova.Validators;

/// <summary>
/// Validator for Moldovan IBANs.
/// </summary>
public class MoldovaIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "MD";

    /// <inheritdoc/>
    protected override int ExpectedLength => 24;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
        => MoldovaBbanValidator.Validate(bban);
}
