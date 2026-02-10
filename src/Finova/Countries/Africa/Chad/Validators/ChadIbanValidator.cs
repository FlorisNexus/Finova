using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Africa.Chad.Validators;

/// <summary>
/// Validator for Chad IBANs.
/// Chad IBAN format: TD + 2 check digits + 23 digits BBAN.
/// Length: 27 characters.
/// </summary>
public class ChadIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "TD";

    /// <inheritdoc/>
    protected override int ExpectedLength => 27;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
        => ChadBbanValidator.Validate(bban);
}
