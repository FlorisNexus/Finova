using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Africa.BurkinaFaso.Validators;

/// <summary>
/// Validator for Burkina Faso IBANs.
/// Burkina Faso IBAN format: BF + 2 check digits + 24 characters (1 letter + 23 digits) BBAN.
/// Length: 28 characters.
/// </summary>
public class BurkinaFasoIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "BF";

    /// <inheritdoc/>
    protected override int ExpectedLength => 28;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
        => BurkinaFasoBbanValidator.Validate(bban);
}
