using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.NorthMacedonia.Validators;

/// <summary>
/// Validator for North Macedonia IBANs.
/// </summary>
public class NorthMacedoniaIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "MK";

    /// <inheritdoc/>
    protected override int ExpectedLength => 19;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
        => NorthMacedoniaBbanValidator.Validate(bban);
}
