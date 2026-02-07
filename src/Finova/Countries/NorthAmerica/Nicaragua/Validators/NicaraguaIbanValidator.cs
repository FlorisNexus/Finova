using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.NorthAmerica.Nicaragua.Validators;

/// <summary>
/// Validator for Nicaraguan IBANs.
/// Nicaragua IBAN format: NI + 2 check digits + 28 characters BBAN (4 letters bank, 24 digits account).
/// </summary>
public class NicaraguaIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "NI";

    /// <inheritdoc/>
    protected override int ExpectedLength => 32;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
    {
        return NicaraguaBbanValidator.Validate(bban);
    }
}
