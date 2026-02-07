using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Africa.Seychelles.Validators;

/// <summary>
/// Validator for Seychelles IBANs.
/// Seychelles IBAN format: SC + 2 check digits + 27 characters BBAN (4 letters bank, 20 digits account, 3 letters currency).
/// Length: 31 characters.
/// </summary>
public class SeychellesIbanValidator : IbanValidatorBase
{
    /// <inheritdoc/>
    public override string CountryCode => "SC";

    /// <inheritdoc/>
    protected override int ExpectedLength => 31;

    /// <inheritdoc/>
    protected override ValidationResult ValidateBban(string bban)
        => SeychellesBbanValidator.Validate(bban);
}
