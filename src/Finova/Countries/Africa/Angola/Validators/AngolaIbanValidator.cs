using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Africa.Angola.Validators;

/// <summary>
/// Validator for Angola IBANs.
/// Angola IBAN format: AO + 2 check digits + 21 digits (BBAN).
/// Length: 25 characters.
/// </summary>
public class AngolaIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "AO";

    /// <inheritdoc/>
    protected override int ExpectedLength => 25;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
        => AngolaBbanValidator.Validate(bban);
}
