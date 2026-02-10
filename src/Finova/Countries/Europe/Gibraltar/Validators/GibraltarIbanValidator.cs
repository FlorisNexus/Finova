using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.Gibraltar.Validators;

/// <summary>
/// Validator for Gibraltarian IBANs.
/// </summary>
public class GibraltarIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "GI";

    /// <inheritdoc/>
    protected override int ExpectedLength => 23;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
    {
        return GibraltarBbanValidator.Validate(bban);
    }
}
