using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Africa.Morocco.Validators;

/// <summary>
/// Validator for Morocco IBANs.
/// Morocco IBAN format: MA + 2 check digits + 24 digits BBAN.
/// Length: 28 characters.
/// </summary>
public class MoroccoIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "MA";

    /// <inheritdoc/>
    protected override int ExpectedLength => 28;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
        => MoroccoBbanValidator.Validate(bban);
}
