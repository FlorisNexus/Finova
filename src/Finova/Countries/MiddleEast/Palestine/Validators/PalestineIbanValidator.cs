using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.MiddleEast.Palestine.Validators;

/// <summary>
/// Validator for Palestinian IBANs.
/// Palestine IBAN format: PS + 2 check digits + 25 characters BBAN (4 letters bank, 21 alphanumeric account).
/// </summary>
public class PalestineIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "PS";

    /// <inheritdoc/>
    protected override int ExpectedLength => 29;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
    {
        return PalestineBbanValidator.Validate(bban);
    }
}
