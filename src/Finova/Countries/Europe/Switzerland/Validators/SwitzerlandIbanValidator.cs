using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.Switzerland.Validators;

/// <summary>
/// Validator for Swiss IBANs.
/// </summary>
public class SwitzerlandIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "CH";

    /// <inheritdoc/>
    protected override int ExpectedLength => 21;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
    {
        return SwitzerlandBbanValidator.Validate(bban);
    }
}
