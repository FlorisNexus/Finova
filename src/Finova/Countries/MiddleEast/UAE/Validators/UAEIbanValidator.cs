using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.MiddleEast.UAE.Validators;

/// <summary>
/// Validator for United Arab Emirates IBANs.
/// UAE IBAN format: AE + 2 check digits + 3 digits (bank code) + 16 digits (account)
/// </summary>
public class UAEIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "AE";

    /// <inheritdoc/>
    protected override int ExpectedLength => 23;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
    {
        return new UaeBbanValidator().Validate(bban);
    }
}
