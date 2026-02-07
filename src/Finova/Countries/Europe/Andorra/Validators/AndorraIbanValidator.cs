using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.Andorra.Validators;

/// <summary>
/// Validator for Andorran IBANs.
/// </summary>
public class AndorraIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "AD";

    /// <inheritdoc/>
    protected override int ExpectedLength => 24;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
    {
        return AndorraBbanValidator.Validate(bban);
    }
}
