using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.MiddleEast.Lebanon.Validators;

/// <summary>
/// Validator for Lebanese IBANs.
/// Lebanon IBAN format: LB + 2 check digits + 4 digits (bank code) + 20 alphanumeric (account)
/// </summary>
public class LebanonIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "LB";

    /// <inheritdoc/>
    protected override int ExpectedLength => 28;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
    {
        return new LebanonBbanValidator().Validate(bban);
    }
}
