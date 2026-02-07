using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.MiddleEast.Kuwait.Validators;

/// <summary>
/// Validator for Kuwaiti IBANs.
/// Kuwait IBAN format: KW + 2 check digits + 4 letters (bank code) + 22 alphanumeric (account)
/// </summary>
public class KuwaitIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "KW";

    /// <inheritdoc/>
    protected override int ExpectedLength => 30;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
    {
        return new KuwaitBbanValidator().Validate(bban);
    }
}
