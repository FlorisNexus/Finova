using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Africa.SaoTomeAndPrincipe.Validators;

/// <summary>
/// Validator for Sao Tome and Principe IBANs.
/// Sao Tome and Principe IBAN format: ST + 2 check digits + 21 digits BBAN.
/// Length: 25 characters.
/// </summary>
public class SaoTomeAndPrincipeIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "ST";

    /// <inheritdoc/>
    protected override int ExpectedLength => 25;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
        => SaoTomeAndPrincipeBbanValidator.Validate(bban);
}
