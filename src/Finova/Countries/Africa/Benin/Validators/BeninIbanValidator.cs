using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Africa.Benin.Validators;

/// <summary>
/// Validator for Benin IBANs.
/// Benin IBAN format: BJ + 2 check digits + 24 characters (1 letter + 23 digits) BBAN.
/// Length: 28 characters.
/// </summary>
public class BeninIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "BJ";

    /// <inheritdoc/>
    protected override int ExpectedLength => 28;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
        => BeninBbanValidator.Validate(bban);
}
