using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.MiddleEast.Iraq.Validators;

/// <summary>
/// Validator for Iraqi IBANs.
/// Iraq IBAN format: IQ + 2 check digits + 19 characters BBAN (4 letters bank, 3 digits branch, 12 digits account).
/// </summary>
public class IraqIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "IQ";

    /// <inheritdoc/>
    protected override int ExpectedLength => 23;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
    {
        return IraqBbanValidator.Validate(bban);
    }
}
