using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.NorthAmerica.SaintLucia.Validators;

/// <summary>
/// Validator for Saint Lucian IBANs.
/// Saint Lucia IBAN format: LC + 2 check digits + 28 characters BBAN (4 letters bank, 24 alphanumeric account).
/// </summary>
public class SaintLuciaIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "LC";

    /// <inheritdoc/>
    protected override int ExpectedLength => 32;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
    {
        return SaintLuciaBbanValidator.Validate(bban);
    }
}
