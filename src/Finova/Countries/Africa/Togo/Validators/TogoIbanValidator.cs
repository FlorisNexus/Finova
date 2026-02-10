using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Africa.Togo.Validators;

/// <summary>
/// Validator for Togo IBANs.
/// Togo IBAN format: TG + 2 check digits + 24 characters (1 letter + 23 digits) BBAN.
/// Length: 28 characters.
/// </summary>
public class TogoIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "TG";

    /// <inheritdoc/>
    protected override int ExpectedLength => 28;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
        => TogoBbanValidator.Validate(bban);
}
