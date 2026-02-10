using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Africa.EquatorialGuinea.Validators;

/// <summary>
/// Validator for Equatorial Guinea IBANs.
/// Equatorial Guinea IBAN format: GQ + 2 check digits + 23 digits BBAN.
/// Length: 27 characters.
/// </summary>
public class EquatorialGuineaIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "GQ";

    /// <inheritdoc/>
    protected override int ExpectedLength => 27;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
        => EquatorialGuineaBbanValidator.Validate(bban);
}
