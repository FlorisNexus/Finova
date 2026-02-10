using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.MiddleEast.Yemen.Validators;

/// <summary>
/// Validator for Yemenite IBANs.
/// Yemen IBAN format: YE + 2 check digits + 26 characters BBAN (4 letters bank, 22 digits account).
/// </summary>
public class YemenIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "YE";

    /// <inheritdoc/>
    protected override int ExpectedLength => 30;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
    {
        return YemenBbanValidator.Validate(bban);
    }
}
