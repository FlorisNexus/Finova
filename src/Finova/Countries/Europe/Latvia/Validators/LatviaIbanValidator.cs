using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.Latvia.Validators;

/// <summary>
/// Validator for Latvian IBANs.
/// </summary>
public class LatviaIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "LV";

    /// <inheritdoc/>
    protected override int ExpectedLength => 21;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
    {
        return LatviaBbanValidator.Validate(bban);
    }
}
