using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.Slovakia.Validators;

/// <summary>
/// Validator for Slovakian IBANs.
/// </summary>
public class SlovakiaIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "SK";

    /// <inheritdoc/>
    protected override int ExpectedLength => 24;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
    {
        return SlovakiaBbanValidator.Validate(bban);
    }
}
