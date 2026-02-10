using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.SouthAmerica.FalklandIslands.Validators;

/// <summary>
/// Validator for Falkland Islands IBANs.
/// Falkland Islands IBAN format: FK + 2 check digits + 14 characters BBAN.
/// </summary>
public class FalklandIslandsIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "FK";

    /// <inheritdoc/>
    protected override int ExpectedLength => 18;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
    {
        return FalklandIslandsBbanValidator.Validate(bban);
    }
}
