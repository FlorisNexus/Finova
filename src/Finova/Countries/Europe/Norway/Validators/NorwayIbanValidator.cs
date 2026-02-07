using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.Norway.Validators;

/// <summary>
/// Validator for Norwegian IBANs.
/// </summary>
public class NorwayIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "NO";

    /// <inheritdoc/>
    protected override int ExpectedLength => 15;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
    {
        return NorwayBbanValidator.Validate(bban);
    }
}
