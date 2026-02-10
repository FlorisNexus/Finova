using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Africa.Somalia.Validators;

/// <summary>
/// Validator for Somalia IBANs.
/// Somalia IBAN format: SO + 2 check digits + 19 digits BBAN.
/// Length: 23 characters.
/// </summary>
public class SomaliaIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "SO";

    /// <inheritdoc/>
    protected override int ExpectedLength => 23;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
        => SomaliaBbanValidator.Validate(bban);
}
