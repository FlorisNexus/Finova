using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.Russia.Validators;

/// <summary>
/// Validator for Russian IBANs.
/// Russia IBAN format: RU + 2 check digits + 29 digits BBAN.
/// </summary>
public class RussiaIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "RU";

    /// <inheritdoc/>
    protected override int ExpectedLength => 33;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
    {
        return RussiaBbanValidator.Validate(bban);
    }
}
