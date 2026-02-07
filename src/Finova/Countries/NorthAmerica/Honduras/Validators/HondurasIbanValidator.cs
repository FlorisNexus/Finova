using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.NorthAmerica.Honduras.Validators;

/// <summary>
/// Validator for Honduran IBANs.
/// Honduras IBAN format: HN + 2 check digits + 24 characters BBAN (4 letters bank, 20 digits account).
/// </summary>
public class HondurasIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "HN";

    /// <inheritdoc/>
    protected override int ExpectedLength => 28;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
    {
        return HondurasBbanValidator.Validate(bban);
    }
}
