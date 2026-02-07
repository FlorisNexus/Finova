using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.MiddleEast.Qatar.Validators;

/// <summary>
/// Validator for Qatari IBANs.
/// Qatar IBAN format: QA + 2 check digits + 4 letters (bank code) + 21 alphanumeric (account)
/// </summary>
public class QatarIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "QA";

    /// <inheritdoc/>
    protected override int ExpectedLength => 29;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
    {
        return new QatarBbanValidator().Validate(bban);
    }
}
