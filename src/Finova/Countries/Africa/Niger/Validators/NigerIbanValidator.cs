using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Africa.Niger.Validators;

/// <summary>
/// Validator for Niger IBANs.
/// Niger IBAN format: NE + 2 check digits + 24 characters (1 letter + 23 digits) BBAN.
/// Length: 28 characters.
/// </summary>
public class NigerIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "NE";

    /// <inheritdoc/>
    protected override int ExpectedLength => 28;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
        => NigerBbanValidator.Validate(bban);
}
