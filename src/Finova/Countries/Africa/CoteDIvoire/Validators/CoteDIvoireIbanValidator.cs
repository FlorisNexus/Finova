using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Africa.CoteDIvoire.Validators;

/// <summary>
/// Validator for Cote D'Ivoire IBANs.
/// Cote D'Ivoire IBAN format: CI + 2 check digits + 24 characters (1 letter + 23 digits) BBAN.
/// Length: 28 characters.
/// </summary>
public class CoteDIvoireIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "CI";

    /// <inheritdoc/>
    protected override int ExpectedLength => 28;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
        => CoteDIvoireBbanValidator.Validate(bban);
}
